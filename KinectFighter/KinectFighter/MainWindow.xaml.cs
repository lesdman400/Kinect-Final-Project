// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using System.Windows.Threading;

using System.Threading;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;




namespace KinectFighter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
                
        public MainWindow()
        {
            InitializeComponent();

        }

        
        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];
        ScaleTransform img = new ScaleTransform();
        DispatcherTimer time = new DispatcherTimer();
        Random random = new Random();

        private bool running = true;
        private DispatcherTimer readyTimer;
        private SpeechRecognitionEngine speechRecognizer;

        KinectSensor ksensor;

        bool movingLabel2 = false;
        bool movingLabel = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            flip();
            title();

            
        }
                   

        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            KinectSensor sensor = (KinectSensor)e.NewValue;

            if (sensor == null)
            {
                return;
            }

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 0.0f
            };
            sensor.SkeletonStream.Enable(parameters);

            sensor.SkeletonStream.Enable();

            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            try
            {
                sensor.Start();
                ksensor = sensor;
                this.InitializeSpeech();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }


        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }


        private void InitializeSpeech()
        {
            this.speechRecognizer = this.CreateSpeechRecognizer();

            if (this.speechRecognizer != null && ksensor != null)
            {
                // NOTE: Need to wait 4 seconds for device to be ready to stream audio right after initialization
                this.readyTimer = new DispatcherTimer();
                this.readyTimer.Tick += this.ReadyTimerTick;
                this.readyTimer.Interval = new TimeSpan(0, 0, 4);
                this.readyTimer.Start();

                this.ReportSpeechStatus("Initializing audio stream...");
                this.UpdateInstructionsText(string.Empty);

                this.Closing += this.MainWindowClosing;
            }

            this.running = true;
        }
        private void ReadyTimerTick(object sender, EventArgs e)
        {
            this.Start();
            this.ReportSpeechStatus("Ready to recognize speech!");
            this.UpdateInstructionsText("Say: 'red', 'green' or 'blue'");
            this.readyTimer.Stop();
            this.readyTimer = null;
        }

        private void UninitializeKinect()
        {
            var sensor = this.ksensor;
            this.running = false;
            if (this.speechRecognizer != null && sensor != null)
            {
                sensor.AudioSource.Stop();
                sensor.Stop();
                this.speechRecognizer.RecognizeAsyncCancel();
                this.speechRecognizer.RecognizeAsyncStop();
            }

            if (this.readyTimer != null)
            {
                this.readyTimer.Stop();
                this.readyTimer = null;
            }
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.UninitializeKinect();
        }

        private SpeechRecognitionEngine CreateSpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();
            if (ri == null)
            {
                MessageBox.Show(
                    @"There was a problem initializing Speech Recognition.
Ensure you have the Microsoft Speech SDK installed.",
                    "Failed to load Speech SDK",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
                return null;
            }

            SpeechRecognitionEngine sre;
            try
            {
                sre = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                MessageBox.Show(
                    @"There was a problem initializing Speech Recognition.
Ensure you have the Microsoft Speech SDK installed and configured.",
                    "Failed to load Speech SDK",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
                return null;
            }

            var colors = new Choices();
            //colors.Add("red");
            //colors.Add("green");
            colors.Add("blue");
            colors.Add("numbers");
            colors.Add("death");
            colors.Add("exit");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(colors);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += this.SreSpeechRecognized;
            sre.SpeechHypothesized += this.SreSpeechHypothesized;
            sre.SpeechRecognitionRejected += this.SreSpeechRecognitionRejected;

            return sre;
        }

        private void RejectSpeech(RecognitionResult result)
        {
            string status = "Rejected: " + (result == null ? string.Empty : result.Text + " " + result.Confidence);
            this.ReportSpeechStatus(status);
            //label1.Content = "REJECTED";
            //Dispatcher.BeginInvoke(new Action(() => { tbColor.Background = blackBrush; }), DispatcherPriority.Normal);
        }

        private void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            this.RejectSpeech(e.Result);
        }

        private void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //label1.Content = "HYPO";
            this.ReportSpeechStatus("Hypothesized: " + e.Result.Text + " " + e.Result.Confidence);
        }

        private void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //SolidColorBrush brush;

            if (e.Result.Confidence < 0.7)
            {
                this.RejectSpeech(e.Result);
                return;
            }

            switch (e.Result.Text.ToUpperInvariant())
            {
                case "NUMBERS":

                    label1.Content = "Not Enough Special Power To Cast Numbers Attack";
                    if (P1Special.Value.Equals(100))
                    {

                        P1SpecialAttack.Visibility = Visibility.Visible;
                        movingLabel = true;
                        numAttack();
                        P2Health.Value -= 30;
                        P1Special.Value = 0;
                        label1.Content = "Casting Numbers Attack";
                    }
                    break;
                case "DEATH":

                    label1.Content = "Not Enough Special Power To Cast Death Attack";
                    if (P2Special.Value.Equals(100))
                    {
                        P2SpecialAttack.Visibility = Visibility.Visible;
                        movingLabel2 = true;
                        boneAttack();
                        P1Health.Value -= 30;
                        P2Special.Value = 0;
                        label1.Content = "Casting Death Attack";


                    }
                    break;
                case "EXIT":
                    
                    Application.Current.Windows[0].Close();
                    
                        closing = true;
                        StopKinect(kinectSensorChooser1.Kinect);
                   
                    break;
                //default:
                //    label1.Content = "other";
                //    break;
            }

            string status = "Recognized: " + e.Result.Text + " " + e.Result.Confidence;
            this.ReportSpeechStatus(status);

//            Dispatcher.BeginInvoke(new Action(() => { tbColor.Background = brush; }), DispatcherPriority.Normal);
        }

        private void ReportSpeechStatus(string status)
        {
            //Dispatcher.BeginInvoke(new Action(() => { tbSpeechStatus.Text = status; }), DispatcherPriority.Normal);
        }

        private void UpdateInstructionsText(string instructions)
        {
            //Dispatcher.BeginInvoke(new Action(() => { tbColor.Text = instructions; }), DispatcherPriority.Normal);
        }

        private void Start()
        {
            var audioSource = this.ksensor.AudioSource;
            var kinectStream = audioSource.Start();
//            this.stream = new EnergyCalculatingPassThroughStream(kinectStream);
            this.speechRecognizer.SetInputToAudioStream(
                kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            this.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
//            var t = new Thread(this.PollSoundSourceLocalization);
//            t.Start();
        }


        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (closing)
            {
                return;
            }

            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);
            Skeleton second = GetSecondSkeleton(e);

            if (first == null)
            {
                return;
            }

            if (movingLabel)
            {
                p1specialattack2.Visibility = Visibility.Visible;
                Canvas.SetLeft(p1specialattack2, Canvas.GetLeft(p1specialattack2) + 50.0);
                if (Canvas.GetLeft(p1specialattack2) > canvas1.Width)
                {
                    movingLabel = false;
                    Canvas.SetLeft(p1specialattack2, -266);
                    p1specialattack2.Visibility = Visibility.Hidden;
                }
            }
            if (movingLabel2)
            {
                p2specialattack2.Visibility = Visibility.Visible;
                Canvas.SetLeft(p2specialattack2, Canvas.GetLeft(p2specialattack2) - 50.0);
                if (Canvas.GetLeft(p2specialattack2) < canvas1.Width/100)
                {
                    movingLabel2 = false;
                    Canvas.SetLeft(p2specialattack2, 0);
                    p2specialattack2.Visibility = Visibility.Hidden;
                }
            }

         
            //set scaled position
            //ScalePosition(headImage, first.Joints[JointType.Head]);
            //ScalePosition(leftEllipse, first.Joints[JointType.HandLeft]);
            //ScalePosition(rightEllipse, first.Joints[JointType.HandRight]);

            GetCameraPoint(first, e);
            GetCameraPoint2(second, e);

           
            PlayerOneHit(P1Lhand, P2Chest);
            PlayerOneHit(P1Rhand, P2Chest);
            PlayerOneHit(P1Lhand, P2Head);
            PlayerOneHit(P1Rhand, P2Head);

            PlayerTwoHit(P2Lhand, P1Chest);
            PlayerTwoHit(P2Rhand, P1Chest);
            PlayerTwoHit(P2Lhand, P1Head);
            PlayerTwoHit(P2Rhand, P1Head);

            winner();
            

        }
/////////////////////////Dectect Winner////////////////////////////////////
        private void winner()
        {
            
            
            
                if (P1Health.Value.Equals(0))
                {
                    p1loses.Visibility = Visibility.Visible;
                    p2wins.Visibility = Visibility.Visible;
                    closing = true;
                    

                }
                if (P2Health.Value.Equals(0))
                {
                    p2loses.Visibility = Visibility.Visible;
                    p1wins.Visibility = Visibility.Visible;
                    closing = true;

                }
            

        }
        void close()
        {
            
        }
////////////////////////////Collision Dectection/////////////////////////////////
        void PlayerOneHit(Image a, Image b)
        {
            
            double P1x = Canvas.GetLeft(a) + a.Width / 2;
            double P1y = Canvas.GetTop(a) + a.Height / 2;
            double P2x = Canvas.GetLeft(b) + b.Width;
            double P2y = Canvas.GetTop(b) + b.Height;

            double radii = a.Height / 2 + b.Height / 2;
            double distance = Math.Sqrt(Math.Pow(P1x - P2x, 2) + Math.Pow(P1y - P2y, 2));
            

            
                if (distance < radii)
                {
                    
                    
                        P2Health.Value -= .5;
                        P1Special.Value += 5;
                        
                    

                }


            }
        
        void PlayerTwoHit(Image c, Image d){

            
            
            double P2x = Canvas.GetLeft(c) + c.Width / 2;
            double P2y = Canvas.GetTop(c) + c.Height / 2;
            double P1x = Canvas.GetLeft(d) + d.Width;
            double P1y = Canvas.GetTop(d) + d.Height;

            double radii = c.Height / 2 + P2Chest.Height / 2;
            double distance = Math.Sqrt(Math.Pow(P2x - P1x, 2) + Math.Pow(P2y - P1y, 2));

            if (distance < radii)
            {

                P1Health.Value -= .5;
                P2Special.Value += 5;

            }
       
        }
///////////////////////Flip Image/////////////////////////////
        private void FlipImage(Image z)
        {


            ScaleTransform img = new ScaleTransform();


            z.RenderTransformOrigin = new Point(0.5, 0.5);
            img.ScaleX = -1;
            z.RenderTransform = img;

        }
        private void flip()
        {
            FlipImage(P2Head);
            FlipImage(P2LArm);
            FlipImage(P2RArm);

            FlipImage(P2LForearm);
            FlipImage(P2RForearm);

            FlipImage(P2Lhand);
            FlipImage(P2Rhand);

            FlipImage(P2LShin);
            FlipImage(P2RShin);

            FlipImage(P2Chest);

            FlipImage(P2LThigh);
            FlipImage(P2RThigh);

            FlipImage(P2Lfoot);
            FlipImage(P2Rfoot);
        }
///////////////////////Rotate Images//////////////////////////
        void rotate(ColorImagePoint part1, ColorImagePoint part2, Image z)
        {

            double angle1 = Math.Atan2((part1.X - part2.X), (part2.Y - part1.Y)) * (180 / Math.PI);
            RotateTransform img = new RotateTransform(angle1 - 90);
            img.CenterX = z.Width / 2;
            img.CenterY = z.Height / 2;
            z.RenderTransform = img;

        }
        void rotateNoNinety(ColorImagePoint part1, ColorImagePoint part2, Image z)
        {

            double angle1 = Math.Atan2((part1.X - part2.X), (part2.Y - part1.Y)) * (180 / Math.PI);
            RotateTransform img = new RotateTransform(angle1);
            img.CenterX = z.Width / 2;
            img.CenterY = z.Height / 2;
            z.RenderTransform = img;

        }
////////////////////// Timer's///////////////////////////////////
        private void title()
        {
            time.Tick+=new EventHandler(time_Tick);
            time.Interval = new TimeSpan(0, 0, 10);
            time.Start();

        }

        void time_Tick(object sender, EventArgs e)
        {
          
            time.Stop();
            fighterImg.Visibility = Visibility.Hidden;
        }
        private void numAttack()
        {
            time.Tick += new EventHandler(time_Tick2);
            time.Interval = new TimeSpan(0, 0, 3);
            time.Start();
            
            

        }

        void time_Tick2(object sender, EventArgs e)
        {
            
            time.Stop();
            P1SpecialAttack.Visibility = Visibility.Hidden;
            label1.Content = " ";
        }
        private void boneAttack()
        {
            time.Tick += new EventHandler(time_Tick3);
            time.Interval = new TimeSpan(0, 0, 3);
            time.Start();


        }

        void time_Tick3(object sender, EventArgs e)
        {

            time.Stop();
            P2SpecialAttack.Visibility = Visibility.Hidden;
            label1.Content = " ";
        }



/////////////////////// Map Skeleton 1 //////////////////////////
        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }


                ///////////////////Map a joint location to a point on the depth map//////////////////////////
                //head
                DepthImagePoint headDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);

                ///////////////Left side/////////////////////
                
                //left wrist
                DepthImagePoint leftWristDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.WristLeft].Position);
               //left ankle
                DepthImagePoint leftAnkleDepthPoint =
               depth.MapFromSkeletonPoint(first.Joints[JointType.AnkleLeft].Position);
               //left hand
                DepthImagePoint leftDepthHandPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                //left elbow
                DepthImagePoint leftElbowDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.ElbowLeft].Position);
                //left hip
                DepthImagePoint leftHipDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HipLeft].Position);
                //left knee
                DepthImagePoint leftKneeDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.KneeLeft].Position);
                //left shoulder
                DepthImagePoint leftShoulderDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.ShoulderLeft].Position);

                ////////////Right side//////////////////
                //right wrist
                DepthImagePoint rightWristDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.WristRight].Position);
                //right ankle
                DepthImagePoint rightAnkleDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.AnkleRight].Position);
                //right hand
                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);
                //right elbow
                DepthImagePoint rightElbowDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.ElbowRight].Position);
                //right hip
                DepthImagePoint rightHipDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HipRight].Position);
                //right knee
                DepthImagePoint rightKneeDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.KneeRight].Position);
                //right shoulder
                DepthImagePoint rightShoulderDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.ShoulderRight].Position);

                //mid
                DepthImagePoint midDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Spine].Position);



                //////////Map a depth point to a point on the color image////////////////////
                //head
                ColorImagePoint headColorPoint =
                    depth.MapToColorImagePoint(headDepthPoint.X , headDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                /////////Left side//////////////
                //left hand
               
                ColorImagePoint leftHandColorPoint =
                    depth.MapToColorImagePoint(leftDepthHandPoint.X, leftDepthHandPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left elbow
                ColorImagePoint leftElbowColorPoint =
                    depth.MapToColorImagePoint(leftElbowDepthPoint.X, leftElbowDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hip
                ColorImagePoint leftHipColorPoint =
                    depth.MapToColorImagePoint(leftHipDepthPoint.X, leftHipDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left knee
                ColorImagePoint leftKneeColorPoint =
                    depth.MapToColorImagePoint(leftKneeDepthPoint.X, leftKneeDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left shoulder
                ColorImagePoint leftShoulderColorPoint =
                    depth.MapToColorImagePoint(leftShoulderDepthPoint.X, leftShoulderDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left ankle
                ColorImagePoint leftAnkleColorPoint =
                    depth.MapToColorImagePoint(leftAnkleDepthPoint.X, leftAnkleDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                /////////////right side////////////////
                //right hand
                ColorImagePoint rightHandColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right elbow
                ColorImagePoint rightElbowColorPoint =
                    depth.MapToColorImagePoint(rightElbowDepthPoint.X, rightElbowDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hip
                ColorImagePoint rightHipColorPoint =
                    depth.MapToColorImagePoint(rightHipDepthPoint.X, rightHipDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right knee
                ColorImagePoint rightKneeColorPoint =
                    depth.MapToColorImagePoint(rightKneeDepthPoint.X, rightKneeDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right shoulder
                ColorImagePoint rightShoulderColorPoint =
                    depth.MapToColorImagePoint(rightShoulderDepthPoint.X, rightShoulderDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right ankle
                ColorImagePoint rightAnkleColorPoint = 
                    depth.MapToColorImagePoint(rightAnkleDepthPoint.X, rightAnkleDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                //mid section
                ColorImagePoint midColorPoint =
                    depth.MapToColorImagePoint(midDepthPoint.X, midDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

        ///////%%%%%%%%%%%%%%%%%%/Newly Added Code for Mapping Images/%%%%%%%%%%%%%%%%%%//////////////
                ColorImagePoint Rarm =
                     depth.MapToColorImagePoint((rightShoulderDepthPoint.X - ((rightShoulderDepthPoint.X - rightElbowDepthPoint.X) / 2)), (rightShoulderDepthPoint.Y - ((rightShoulderDepthPoint.Y - rightElbowDepthPoint.Y) / 2)), 
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Larm =
                     depth.MapToColorImagePoint((leftShoulderDepthPoint.X - ((leftShoulderDepthPoint.X - leftElbowDepthPoint.X) / 2)), (leftShoulderDepthPoint.Y - ((leftShoulderDepthPoint.Y - leftElbowDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint FRarm =
                    depth.MapToColorImagePoint((rightElbowDepthPoint.X - ((rightElbowDepthPoint.X - rightWristDepthPoint.X) / 2)), (rightElbowDepthPoint.Y - ((rightElbowDepthPoint.Y - rightWristDepthPoint.Y) / 2)),
                    ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint FLarm =
                     depth.MapToColorImagePoint((leftElbowDepthPoint.X - ((leftElbowDepthPoint.X - leftWristDepthPoint.X) / 2)), (leftElbowDepthPoint.Y - ((leftElbowDepthPoint.Y - leftWristDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Rthigh =
                     depth.MapToColorImagePoint((rightHipDepthPoint.X - ((rightHipDepthPoint.X - rightKneeDepthPoint.X) / 2)), (rightHipDepthPoint.Y - ((rightHipDepthPoint.Y - rightKneeDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Lthigh =
                     depth.MapToColorImagePoint((leftHipDepthPoint.X - ((leftHipDepthPoint.X - leftKneeDepthPoint.X) / 2)), (leftHipDepthPoint.Y - ((leftHipDepthPoint.Y - leftKneeDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Rshin =
                     depth.MapToColorImagePoint((rightKneeDepthPoint.X - ((rightKneeDepthPoint.X - rightAnkleDepthPoint.X) / 2)), (rightKneeDepthPoint.Y - ((rightKneeDepthPoint.Y - rightAnkleDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Lshin =
                     depth.MapToColorImagePoint((leftKneeDepthPoint.X - ((leftKneeDepthPoint.X - leftAnkleDepthPoint.X) / 2)), (leftKneeDepthPoint.Y - ((leftKneeDepthPoint.Y - leftAnkleDepthPoint.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);


                //Set location
                
                CameraPosition(P1Head, headColorPoint);
                
                CameraPosition(P1Rhand, rightHandColorPoint);
                CameraPosition(P1Lhand, leftHandColorPoint);

                CameraPosition(P1Chest, midColorPoint);

                CameraPosition(P1RArm, Rarm);
                CameraPosition(P1LArm, Larm);

                CameraPosition(P1RForearm, FRarm);
                CameraPosition(P1LForearm, FLarm);

                CameraPosition(P1RShin, Rshin);
                CameraPosition(P1LShin, Lshin);

                CameraPosition(P1RThigh, Rthigh);
                CameraPosition(P1LThigh, Lthigh);

                CameraPosition(P1Rfoot, rightAnkleColorPoint);
                CameraPosition(P1Lfoot, leftAnkleColorPoint);

              
                rotate(leftHandColorPoint, leftElbowColorPoint, P1LForearm);
                rotate(rightHandColorPoint, rightElbowColorPoint, P1RForearm);

                rotate(leftElbowColorPoint, leftShoulderColorPoint, P1LArm);
                rotate(rightElbowColorPoint, rightShoulderColorPoint, P1RArm);

                rotateNoNinety(leftKneeColorPoint, leftHipColorPoint, P1LThigh);
                rotateNoNinety(rightKneeColorPoint, rightHipColorPoint, P1RThigh);

                rotateNoNinety(leftAnkleColorPoint, leftKneeColorPoint, P1LShin);
                rotateNoNinety(rightAnkleColorPoint, rightKneeColorPoint, P1RShin);

                
           

            }
        }




        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;

            }
        }

//////////////////////////////Map Skeleton 2///////////////////////////////
        void GetCameraPoint2(Skeleton second, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }


                ///////////////////Map a joint location to a point on the depth map//////////////////////////
                //head
                DepthImagePoint headDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.Head].Position);

                ///////////////Left side/////////////////////
                //left wrist
                DepthImagePoint leftWristDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.WristLeft].Position);
                //left hand
                DepthImagePoint leftDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.HandLeft].Position);
                //left elbow
                DepthImagePoint leftElbowDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.ElbowLeft].Position);
                //left hip
                DepthImagePoint leftHipDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.HipLeft].Position);
                //left knee
                DepthImagePoint leftKneeDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.KneeLeft].Position);
                //left shoulder
                DepthImagePoint leftShoulderDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.ShoulderLeft].Position);
                //left ankle
                DepthImagePoint leftAnkleDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.AnkleLeft].Position);

                ////////////Right side//////////////////
                //right ankle
                DepthImagePoint rightAnkleDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.AnkleRight].Position);
                //right wrist
                DepthImagePoint rightWristDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.WristRight].Position);
                //right hand
                DepthImagePoint rightDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.HandRight].Position);
                //right elbow
                DepthImagePoint rightElbowDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.ElbowRight].Position);
                //right hip
                DepthImagePoint rightHipDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.HipRight].Position);
                //right knee
                DepthImagePoint rightKneeDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.KneeRight].Position);
                //right shoulder
                DepthImagePoint rightShoulderDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.ShoulderRight].Position);

                //mid
                DepthImagePoint midDepthPoint1 =
                    depth.MapFromSkeletonPoint(second.Joints[JointType.Spine].Position);



                //////////Map a depth point to a point on the color image////////////////////
                //head
                ColorImagePoint headColorPoint1 =
                    depth.MapToColorImagePoint(headDepthPoint1.X +35, headDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                /////////Left side//////////////
                //left ankle
                ColorImagePoint leftAnkleColorPoint1 =
                    depth.MapToColorImagePoint(leftAnkleDepthPoint1.X, leftAnkleDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                //left hand
                ColorImagePoint leftHandColorPoint1 =
                    depth.MapToColorImagePoint(leftDepthPoint1.X, leftDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left elbow
                ColorImagePoint leftElbowColorPoint1 =
                    depth.MapToColorImagePoint(leftElbowDepthPoint1.X, leftElbowDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hip
                ColorImagePoint leftHipColorPoint1 =
                    depth.MapToColorImagePoint(leftHipDepthPoint1.X, leftHipDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left knee
                ColorImagePoint leftKneeColorPoint1 =
                    depth.MapToColorImagePoint(leftKneeDepthPoint1.X, leftKneeDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left shoulder
                ColorImagePoint leftShoulderColorPoint1 =
                    depth.MapToColorImagePoint(leftShoulderDepthPoint1.X, leftShoulderDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                /////////////right side////////////////
                //right ankle
                ColorImagePoint rightAnkleColorPoint1 =
                    depth.MapToColorImagePoint(rightAnkleDepthPoint1.X, rightAnkleDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hand
                ColorImagePoint rightHandColorPoint1 =
                    depth.MapToColorImagePoint(rightDepthPoint1.X + 40, rightDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left elbow
                ColorImagePoint rightElbowColorPoint1 =
                    depth.MapToColorImagePoint(rightElbowDepthPoint1.X, rightElbowDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hip
                ColorImagePoint rightHipColorPoint1 =
                    depth.MapToColorImagePoint(rightHipDepthPoint1.X, rightHipDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left knee
                ColorImagePoint rightKneeColorPoint1 =
                    depth.MapToColorImagePoint(rightKneeDepthPoint1.X, rightKneeDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left shoulder
                ColorImagePoint rightShoulderColorPoint1 =
                    depth.MapToColorImagePoint(rightShoulderDepthPoint1.X, rightShoulderDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                //mid section
                ColorImagePoint midColorPoint1 =
                    depth.MapToColorImagePoint(midDepthPoint1.X, midDepthPoint1.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                ///////%%%%%%%%%%%%%%%%%%/Newly Added Code for Mapping Images/%%%%%%%%%%%%%%%%%%//////////////
                ColorImagePoint Rarm1 =
                     depth.MapToColorImagePoint((rightShoulderDepthPoint1.X - ((rightShoulderDepthPoint1.X - rightElbowDepthPoint1.X) / 2)), (rightShoulderDepthPoint1.Y - ((rightShoulderDepthPoint1.Y - rightElbowDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Larm1 =
                     depth.MapToColorImagePoint((leftShoulderDepthPoint1.X - ((leftShoulderDepthPoint1.X - leftElbowDepthPoint1.X) / 2)), (leftShoulderDepthPoint1.Y - ((leftShoulderDepthPoint1.Y - leftElbowDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint FRarm1 =
                    depth.MapToColorImagePoint((rightElbowDepthPoint1.X - ((rightElbowDepthPoint1.X - rightWristDepthPoint1.X) / 2)), (rightElbowDepthPoint1.Y - ((rightElbowDepthPoint1.Y - rightWristDepthPoint1.Y) / 2)),
                    ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint FLarm1 =
                     depth.MapToColorImagePoint((leftElbowDepthPoint1.X - ((leftElbowDepthPoint1.X - leftWristDepthPoint1.X) / 2)), (leftElbowDepthPoint1.Y - ((leftElbowDepthPoint1.Y - leftWristDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Rthigh1 =
                     depth.MapToColorImagePoint((rightHipDepthPoint1.X - ((rightHipDepthPoint1.X - rightKneeDepthPoint1.X) / 2)), (rightHipDepthPoint1.Y - ((rightHipDepthPoint1.Y - rightKneeDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Lthigh1 =
                     depth.MapToColorImagePoint((leftHipDepthPoint1.X - ((leftHipDepthPoint1.X - leftKneeDepthPoint1.X) / 2)), (leftHipDepthPoint1.Y - ((leftHipDepthPoint1.Y - leftKneeDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Rshin1 =
                     depth.MapToColorImagePoint((rightKneeDepthPoint1.X - ((rightKneeDepthPoint1.X - rightAnkleDepthPoint1.X) / 2)), (rightKneeDepthPoint1.Y - ((rightKneeDepthPoint1.Y - rightAnkleDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint Lshin1 =
                     depth.MapToColorImagePoint((leftKneeDepthPoint1.X - ((leftKneeDepthPoint1.X - leftAnkleDepthPoint1.X) / 2)), (leftKneeDepthPoint1.Y - ((leftKneeDepthPoint1.Y - leftAnkleDepthPoint1.Y) / 2)),
                     ColorImageFormat.RgbResolution640x480Fps30);

                rotate(leftHandColorPoint1, leftElbowColorPoint1, P2LForearm);
                rotate(rightHandColorPoint1, rightElbowColorPoint1, P2RForearm);

                rotate(leftElbowColorPoint1, leftShoulderColorPoint1, P2LArm);
                rotate(rightElbowColorPoint1, rightShoulderColorPoint1, P2RArm);

                rotateNoNinety(leftKneeColorPoint1, leftHipColorPoint1, P2LThigh);
                rotateNoNinety(rightKneeColorPoint1, rightHipColorPoint1, P2RThigh);

                rotateNoNinety(leftAnkleColorPoint1, leftKneeColorPoint1, P2LShin);
                rotateNoNinety(rightAnkleColorPoint1, rightKneeColorPoint1, P2RShin);

                CameraPosition2(P2Head, headColorPoint1);

                CameraPosition2(P2Rhand, rightHandColorPoint1);
                CameraPosition2(P2Lhand, leftHandColorPoint1);

                CameraPosition(P2Chest, midColorPoint1);

                CameraPosition2(P2RArm, Rarm1);
                CameraPosition2(P2LArm, Larm1);

                CameraPosition2(P2RForearm, FRarm1);
                CameraPosition2(P2LForearm, FLarm1);

                CameraPosition3(P2RShin, Rshin1);
                CameraPosition3(P2LShin, Lshin1);

                CameraPosition3(P2RThigh, Rthigh1);
                CameraPosition3(P2LThigh, Lthigh1);

                CameraPosition2(P2Rfoot, rightAnkleColorPoint1);
                CameraPosition2(P2Lfoot, leftAnkleColorPoint1);

                

                

            }
        }
        Skeleton GetSecondSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton second = (from s in allSkeletons
                                   where s.TrackingState == SkeletonTrackingState.Tracked
                                   select s).LastOrDefault();

                return second;

            }
        }
/////////////Stop Kinect Method////////////////////////////
        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //stop audio if not null
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }


                }
            }
        }
////////////////////Set Images away from Skeleton/////////////////////////////////
        private void CameraPosition(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X + 20 - element.Width / 2);
            Canvas.SetTop(element, point.Y - element.Height / 2);

        }
        private void CameraPosition2(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X - 20 - element.Width / 2);
            Canvas.SetTop(element, point.Y  - element.Height / 2);

        }
        private void CameraPosition3(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X - 20 - element.Width / 2);
            Canvas.SetTop(element, point.Y -60 - element.Height / 2);

        }
//////////////////Scale Images////////////////////////////
        private void ScalePosition(FrameworkElement element, Joint joint)
        {
            //convert the value to X/Y
            //Joint scaledJoint = joint.ScaleTo(1280, 720); 

            //convert & scale (.3 = means 1/3 of joint distance)
            Joint scaledJoint = joint.ScaleTo(1280, 720, .3f, .3f);

            Canvas.SetLeft(element, scaledJoint.Position.X);
            Canvas.SetTop(element, scaledJoint.Position.Y);

        }

///////////////Loading & Closing Methods//////////////////
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect);
        }

        private void kinectColorViewer1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
  








    }
}
