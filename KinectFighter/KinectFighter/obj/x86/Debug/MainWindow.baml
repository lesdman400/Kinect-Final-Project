   M S B A M L   `   `   `  ����^  ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers  5   6    KinectFighter   KinectFighter.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     5   6   -   5          � Title$  
MainWindow��5   	   $	��666��6   $
��1112��6)    � 
ResizeMode$ NoResize=�6�    � WindowState$ 	Maximized=�6�   .��5      �� -   5        $��canvas1��5      $	��624��6   $
��1074��6)   .+�5   
   �� -   5        $��P2SpecialAttack��5   A    �� Left$
 -132��6   
 �� Top$	 -78��6#   $	��734��64   $
��Fill=�6X   $
��1370��6g   $6��0/KinectFighter;component/Images/scary-skulls.jpg��6t   $y�Hidden=�6�   �� 5   
   -   5        $��P1SpecialAttack��5   @   $	 -34��6   $	 -46��6"   $	��702��63   $
��Fill=�6W   $
��1148��6f   $1��+/KinectFighter;component/Images/special.jpg��6s   $y�Hidden=�6�   �� 5   
   -   5        $��P1Head��5   =   $ 3��6   $	 190��6    $��77��61   $
��Fill=�6K   $��86��6Z   $1��+/KinectFighter;component/Images/numhead.png��6e   �� 5	   
   -   5        $��P1Lfoot��5	   >   $ 10��6   $	 498��6!   $��64��62   $
��Fill=�6M   $��59��6\   $1��+/KinectFighter;component/Images/numfoot.png��6g   �� 5
   
   -   5        $��P1Rfoot��5
   >   $ 59��6   $	 498��6!   $��64��62   $1��+/KinectFighter;component/Images/numfoot.png��6M   $
��Fill=�6�   $��59��6�   �� 5   
   -   5        $��P1Chest��5   =   $ 3��6   $	 272��6    $��64��61   $1��+/KinectFighter;component/Images/numbody.png��6L   $
��Fill=�6�   $��79��6�   �� 5   
   -	   5        $��P1LArm��5   >   $ 83��6   $	 272��6!   $��48��62   $0��*/KinectFighter;component/Images/numarm.png��6L   $
��Fill=�6�   $��79��6�   �� 5   
   -
   5        $��
P1LForearm��5   ?   $	 137��6   $	 272��6"   $��49��63   $0��*/KinectFighter;component/Images/numarm.png��6Q   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��P1RArm��5   >   $ 83��6   $	 342��6!   $��41��62   $0��*/KinectFighter;component/Images/numarm.png��6L   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��
P1RForearm��5   ?   $	 137��6   $	 342��6"   $��41��63   $0��*/KinectFighter;component/Images/numarm.png��6Q   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��P1Lhand��5   ?   $	 222��6   $	 256��6"   $��64��63   $1��+/KinectFighter;component/Images/numfist.png��6N   $
��Fill=�6�   $��47��6�   �� 5   
   -   5        $��P1Rhand��5   ?   $	 222��6   $	 328��6"   $��56��63   $1��+/KinectFighter;component/Images/numfist.png��6N   $
��Fill=�6�   $��47��6�   �� 5   
   -   5        $��P1LThigh��5   >   $ 12��6   $	 342��6!   $��89��62   $0��*/KinectFighter;component/Images/numleg.png��6N   $
��Fill=�6�   $��31��6�   �� 5   
   -   5        $��P1RThigh��5   >   $ 52��6   $	 342��6!   $��87��62   $0��*/KinectFighter;component/Images/numleg.png��6N   $
��Fill=�6�   $��37��6�   �� 5   
   -   5        $��P1LShin��5   >   $ 10��6   $	 428��6!   $��64��62   $0��*/KinectFighter;component/Images/numleg.png��6M   $
��Fill=�6�   $��33��6�   �� 5   
   -   5        $��P1RShin��5   >   $ 52��6   $	 428��6!   $��64��62   $0��*/KinectFighter;component/Images/numleg.png��6M   $
��Fill=�6�   $��41��6�   �� 5   
   -   5        $��P2Head��5   ?   $	 920��6   $	 190��6"   $��77��63   $4��./KinectFighter;component/Images/CleanSkull.png��6M   $
��Fill=�6�   $��86��6�   �� 5   
   -   5        $��P2Lfoot��5   ?   $	 902��6   $	 488��6"   $��64��63   $2��,/KinectFighter;component/Images/BoneFoot.png��6N   $
��Fill=�6�   $��59��6�   �� 5   
   -   5        $��P2Rfoot��5   ?   $	 952��6   $	 488��6"   $��64��63   $2��,/KinectFighter;component/Images/BoneFoot.png��6N   $
��Fill=�6�   $��59��6�   �� 5   
   -   5        $��P2Chest��5   ?   $	 928��6   $	 272��6"   $��64��63   $2��,/KinectFighter;component/Images/CleanRib.png��6N   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��P2LArm��5   ?   $	 834��6   $	 256��6"   $��48��63   $2��,/KinectFighter;component/Images/ArmBone1.png��6M   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��
P2LForearm��5   ?   $	 866��6   $	 258��6"   $��48��63   $2��,/KinectFighter;component/Images/ArmBone1.png��6Q   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��P2RArm��5   ?   $	 830��6   $	 304��6"   $��49��63   $2��,/KinectFighter;component/Images/ArmBone1.png��6M   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��
P2RForearm��5   ?   $	 858��6   $	 302��6"   $��49��63   $2��,/KinectFighter;component/Images/ArmBone1.png��6Q   $
��Fill=�6�   $��79��6�   �� 5   
   -   5        $��P2Lhand��5   ?   $	 782��6   $	 272��6"   $��64��63   $3��-/KinectFighter;component/Images/CleanHand.png��6N   $
��Fill=�6�   $��47��6�   �� 5   
   -   5        $��P2Rhand��5   ?   $	 782��6   $	 212��6"   $��56��63   $3��-/KinectFighter;component/Images/CleanHand.png��6N   $
��Fill=�6�   $��47��6�   �� 5    
   -   5        $��P2LThigh��5    ?   $	 930��6   $	 342��6"   $��89��63   $2��,/KinectFighter;component/Images/LegBone1.png��6O   $
��Fill=�6�   $��31��6�   �� 5!   
   -   5        $��P2RThigh��5!   ?   $	 970��6   $	 342��6"   $��87��63   $2��,/KinectFighter;component/Images/LegBone1.png��6O   $
��Fill=�6�   $��37��6�   �� 5"   
   -   5        $��P2LShin��5"   ?   $	 928��6   $	 428��6"   $��64��63   $2��,/KinectFighter;component/Images/LegBone1.png��6N   $
��Fill=�6�   $��33��6�   �� 5#   
   -    5        $��P2RShin��5#   ?   $	 970��6   $	 428��6"   $��64��63   $2��,/KinectFighter;component/Images/LegBone1.png��6N   $
��Fill=�6�   $��41��6�   =   7Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser  5$   
   -!   5        $��kinectSensorChooser1��5$   D   $	 370��6!   $	 334��63   $	��328��6`   � 5%   
   -"   5        $��P1Health��5%   C   $ 94��6   $ 58��6'   $��20��67   $	��200��6S    � Value$	 100Q�6_   
���  ��6k   � 5&   
   -#   5        $��P2Health��5&   D   $	 782��6   $ 58��6(   $��20��68   $	��200��6T   $	 100Q�6`   
���  ��6l   � 5'   
   -$   5        $��	P1Special��5'   C   $ 94��6   $ 94��6'   $��12��67   $	��129��6T   
��� ���$	 100Q�6t   � 5(   
   -%   5        $��	P2Special��5(   D   $	 854��6   $ 94��6(   $��12��68   $	��129��6U   
��� ���$	 100Q�6u   �� 5)   
   -&   5        $��P1Name��5)   ;   $ 0��6   $ 2��6    $��50��6/   $
��Fill=�6I   $	��346��6X   $?��9/KinectFighter;component/Images/coollogo_com-31037549.png��6d   �� 5*   
   -'   5        $��P2Name��5*   =   $	 728��6   $ 2��6"   $��50��61   $@��:/KinectFighter;component/Images/coollogo_com-310551516.png��6K   $
��Fill=�6�   $	��346��6�   �� 5+   
   -(   5        $��vsImage��5+   ?   $	 438��6   $ 12��6"   $	��122��62   $
��Fill=�6N   $	��196��6]   $?��9/KinectFighter;component/Images/coollogo_com-28353614.png��6i   �� 5,   
   -)   5        $��P1SpecialImage��5,   =   $ 38��6   $ 82��6!   $��40��61   $
��Fill=�6S   $��66��6b   $@��:/KinectFighter;component/Images/coollogo_com-310291456.png��6m   �� 5-   
   -*   5        $��P2SpecialImage��5-   >   $	 974��6   $ 82��6"   $��40��62   $@��:/KinectFighter;component/Images/coollogo_com-310291456.png��6T   $
��Fill=�6�   $��66��6�   �� 5.   
   -+   5        $��P1HealthImage��5.   <   $ 0��6   $ 48��6    $��42��60   $
��Fill=�6Q   $	��105��6`   $?��9/KinectFighter;component/Images/coollogo_com-28353991.png��6l   �� 5/   
   -,   5        $��P2HealthImage��5/   >   $	 969��6   $ 48��6"   $��42��62   $?��9/KinectFighter;component/Images/coollogo_com-28353991.png��6S   $
��Fill=�6�   $	��105��6�   �� 50   
   --   5        $��label1��50   =   $ 0��6   $	 620��6    $��70��61   $
��1074��6K   
���   �$��32(�6k   $y�Visible=�6y   
���  ��$��Center=�6�   $��Center=�6�   �� 51   
   -.   5        $��p1wins��51   B   $
 -230��6   $
 -105��6#   $	��534��65   $
��Fill=�6P   $
��1565��6_   $@��:/KinectFighter;component/Images/coollogo_com-175647945.png��6l   $y�Hidden=�6�   �� 52   
   -/   5        $��p2wins��52   B   $
 -230��6   $
 -103��6#   $	��534��65   $A��;/KinectFighter;component/Images/coollogo_com-1755610872.png��6P   $
��Fill=�6�   $y�Hidden=�6�   $
��1565��6�   �� 53   
   -0   5        $��p1loses��53   A   $
 -112��6   $	 190��6#   $	��530��64   $@��:/KinectFighter;component/Images/coollogo_com-280405083.png��6P   $
��Fill=�6�   $y�Hidden=�6�   $
��1350��6�   �� 54   
   -1   5        $��p2loses��54   A   $
 -112��6   $	 190��6#   $	��530��64   $
��Fill=�6P   $
��1292��6_   $y�Hidden=�6l   $@��:/KinectFighter;component/Images/coollogo_com-280405102.png��6�   �� 55   
   -2   5        $��p1specialattack2��55   A   $
 -378��6   $	 190��6#   $	��150��64   $
��Fill=�6Y   $	��384��6h   $0��*/KinectFighter;component/Images/numarm.png��6t   $y�Hidden=�6�   �� 56   
   -3   5        $��p2specialattack2��56   A   $
 1148��6   $	 302��6#   $	��150��64   $
��Fill=�6Y   $	��360��6h   $2��,/KinectFighter;component/Images/ArmBone1.png��6t   $y�Hidden=�6�   �� 57   
   -4   5        $��
fighterImg��57   A   $
 -112��6   $	 -78��6#   $	��798��64   $
��Fill=�6S   $
��1292��6b   $A��;/KinectFighter;component/Images/coollogo_com-2835110142.png��6o   $y�Visible=�6�   ��58   
   �� 59       �� ImageSource$/ )/KinectFighter;component/Images/fist1.jpg��6   5;      5<      