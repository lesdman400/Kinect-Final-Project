   M S B A M L   `   `   `  ���� ^  ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers   KinectFighter   KinectFighter.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     -     � Title$  
MainWindow��$	��666��$
��1112�� � 
ResizeMode$ NoResize=� � WindowState$ 	Maximized=�.���� -   $��canvas1��$	��624��$
��1074��.+��� -   $��P2SpecialAttack�� �� Left$
 -132��
 �� Top$	 -78��$	��734��$
��Fill=�$
��1370��$6��0/KinectFighter;component/Images/scary-skulls.jpg��$y�Hidden=��� -   $��P1SpecialAttack��$	 -34��$	 -46��$	��702��$
��Fill=�$
��1148��$1��+/KinectFighter;component/Images/special.jpg��$y�Hidden=��� -   $��P1Head��$ 3��$	 190��$��77��$
��Fill=�$��86��$1��+/KinectFighter;component/Images/numhead.png���� -   $��P1Lfoot��$ 10��$	 498��$��64��$
��Fill=�$��59��$1��+/KinectFighter;component/Images/numfoot.png���� -   $��P1Rfoot��$ 59��$	 498��$��64��$1��+/KinectFighter;component/Images/numfoot.png��$
��Fill=�$��59���� -   $��P1Chest��$ 3��$	 272��$��64��$1��+/KinectFighter;component/Images/numbody.png��$
��Fill=�$��79���� -	   $��P1LArm��$ 83��$	 272��$��48��$0��*/KinectFighter;component/Images/numarm.png��$
��Fill=�$��79���� -
   $��
P1LForearm��$	 137��$	 272��$��49��$0��*/KinectFighter;component/Images/numarm.png��$
��Fill=�$��79���� -   $��P1RArm��$ 83��$	 342��$��41��$0��*/KinectFighter;component/Images/numarm.png��$
��Fill=�$��79���� -   $��
P1RForearm��$	 137��$	 342��$��41��$0��*/KinectFighter;component/Images/numarm.png��$
��Fill=�$��79���� -   $��P1Lhand��$	 222��$	 256��$��64��$1��+/KinectFighter;component/Images/numfist.png��$
��Fill=�$��47���� -   $��P1Rhand��$	 222��$	 328��$��56��$1��+/KinectFighter;component/Images/numfist.png��$
��Fill=�$��47���� -   $��P1LThigh��$ 12��$	 342��$��89��$0��*/KinectFighter;component/Images/numleg.png��$
��Fill=�$��31���� -   $��P1RThigh��$ 52��$	 342��$��87��$0��*/KinectFighter;component/Images/numleg.png��$
��Fill=�$��37���� -   $��P1LShin��$ 10��$	 428��$��64��$0��*/KinectFighter;component/Images/numleg.png��$
��Fill=�$��33���� -   $��P1RShin��$ 52��$	 428��$��64��$0��*/KinectFighter;component/Images/numleg.png��$
��Fill=�$��41���� -   $��P2Head��$	 920��$	 190��$��77��$4��./KinectFighter;component/Images/CleanSkull.png��$
��Fill=�$��86���� -   $��P2Lfoot��$	 902��$	 488��$��64��$2��,/KinectFighter;component/Images/BoneFoot.png��$
��Fill=�$��59���� -   $��P2Rfoot��$	 952��$	 488��$��64��$2��,/KinectFighter;component/Images/BoneFoot.png��$
��Fill=�$��59���� -   $��P2Chest��$	 928��$	 272��$��64��$2��,/KinectFighter;component/Images/CleanRib.png��$
��Fill=�$��79���� -   $��P2LArm��$	 834��$	 256��$��48��$2��,/KinectFighter;component/Images/ArmBone1.png��$
��Fill=�$��79���� -   $��
P2LForearm��$	 866��$	 258��$��48��$2��,/KinectFighter;component/Images/ArmBone1.png��$
��Fill=�$��79���� -   $��P2RArm��$	 830��$	 304��$��49��$2��,/KinectFighter;component/Images/ArmBone1.png��$
��Fill=�$��79���� -   $��
P2RForearm��$	 858��$	 302��$��49��$2��,/KinectFighter;component/Images/ArmBone1.png��$
��Fill=�$��79���� -   $��P2Lhand��$	 782��$	 272��$��64��$3��-/KinectFighter;component/Images/CleanHand.png��$
��Fill=�$��47���� -   $��P2Rhand��$	 782��$	 212��$��56��$3��-/KinectFighter;component/Images/CleanHand.png��$
��Fill=�$��47���� -   $��P2LThigh��$	 930��$	 342��$��89��$2��,/KinectFighter;component/Images/LegBone1.png��$
��Fill=�$��31���� -   $��P2RThigh��$	 970��$	 342��$��87��$2��,/KinectFighter;component/Images/LegBone1.png��$
��Fill=�$��37���� -   $��P2LShin��$	 928��$	 428��$��64��$2��,/KinectFighter;component/Images/LegBone1.png��$
��Fill=�$��33���� -    $��P2RShin��$	 970��$	 428��$��64��$2��,/KinectFighter;component/Images/LegBone1.png��$
��Fill=�$��41��=   7Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser  -!   $��kinectSensorChooser1��$	 370��$	 334��$	��328��� -"   $��P1Health��$ 94��$ 58��$��20��$	��200�� � Value$	 100Q�
���  ��� -#   $��P2Health��$	 782��$ 58��$��20��$	��200��$	 100Q�
���  ��� -$   $��	P1Special��$ 94��$ 94��$��12��$	��129��
��� ���$	 100Q�� -%   $��	P2Special��$	 854��$ 94��$��12��$	��129��
��� ���$	 100Q��� -&   $��P1Name��$ 0��$ 2��$��50��$
��Fill=�$	��346��$?��9/KinectFighter;component/Images/coollogo_com-31037549.png���� -'   $��P2Name��$	 728��$ 2��$��50��$@��:/KinectFighter;component/Images/coollogo_com-310551516.png��$
��Fill=�$	��346���� -(   $��vsImage��$	 438��$ 12��$	��122��$
��Fill=�$	��196��$?��9/KinectFighter;component/Images/coollogo_com-28353614.png���� -)   $��P1SpecialImage��$ 38��$ 82��$��40��$
��Fill=�$��66��$@��:/KinectFighter;component/Images/coollogo_com-310291456.png���� -*   $��P2SpecialImage��$	 974��$ 82��$��40��$@��:/KinectFighter;component/Images/coollogo_com-310291456.png��$
��Fill=�$��66���� -+   $��P1HealthImage��$ 0��$ 48��$��42��$
��Fill=�$	��105��$?��9/KinectFighter;component/Images/coollogo_com-28353991.png���� -,   $��P2HealthImage��$	 969��$ 48��$��42��$?��9/KinectFighter;component/Images/coollogo_com-28353991.png��$
��Fill=�$	��105���� --   $��
fighterImg��$	 -34��$	 185��$	��166��$
��Fill=�$
��1148��$@��:/KinectFighter;component/Images/coollogo_com-283511014.png��$y�Visible=��� -.   $��label1��$ 0��$	 620��$��70��$
��1074��
���   �$��32(�$y�Visible=�
���  ��$��Center=�$��Center=��� -/   $��p1wins��$
 -230��$
 -105��$	��534��$
��Fill=�$
��1565��$@��:/KinectFighter;component/Images/coollogo_com-175647945.png��$y�Hidden=��� -0   $��p2wins��$
 -230��$
 -103��$	��534��$A��;/KinectFighter;component/Images/coollogo_com-1755610872.png��$
��Fill=�$y�Hidden=�$
��1565���� -1   $��p1loses��$
 -112��$	 190��$	��530��$@��:/KinectFighter;component/Images/coollogo_com-280405083.png��$
��Fill=�$y�Hidden=�$
��1350���� -2   $��p2loses��$
 -112��$	 190��$	��530��$
��Fill=�$
��1292��$y�Hidden=�$@��:/KinectFighter;component/Images/coollogo_com-280405102.png���� -3   $��p1specialattack2��$
 -378��$	 190��$	��150��$
��Fill=�$	��384��$0��*/KinectFighter;component/Images/numarm.png��$y�Hidden=��� -4   $��p2specialattack2��$
 1148��$	 302��$	��150��$
��Fill=�$	��360��$2��,/KinectFighter;component/Images/ArmBone1.png��$y�Hidden=�����  �� ImageSource$/ )/KinectFighter;component/Images/fist1.jpg��