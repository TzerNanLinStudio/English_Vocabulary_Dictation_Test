   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:UIUI  5   	   T PEmgu.CV.UI, Version=4.1.1.3497, Culture=neutral, PublicKeyToken=7281126722ab4438;,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI
Emgu.CV.UI 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	       UI.ChoiceList   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   "localclr-namespace:UI     5   	   =emgu,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5        $��12(�5
   	   $��Microsoft JhengHei)�5   	   
����·�  � WindowStartupLocation$  CenterScreen=�5   	    � Title$ 選擇清單��5   	   $	��100��6   $	��300��6#    � 	Resources 5         buttonHover �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius	 �� Triggers
 P� Property P� Value �� 
TargetName��%  &	        �� 5   
   " ��6$   .�5      ��  � �� �#8a8c8e6+   �� 5       � ��
 �   �6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ border��6!   #��z��61   $��3q�6[   
���   �$ 3��6�   ._�5      �� $��Center=�6/   $��Center=�6L   5      	 5      P� 5      
 � �� . . �5   "   �� $ border��6)    � ��$ 5q�6X   5      P� 5      
 � �� . . �5   "   ��  � �� �#d3d7d46?   5       5"      5$      5%      5&      .��5(      � .+�5)   
   �� 
����·�$��0q�6&   $��-1,-1,-1,-1q�6:   ._�5*      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�6M   .+�5+      �� -   5         ��Name$ Btn_CreatNewText��5+      $��75��63   $��25��6>   #��[  6\   ��5,      �� 5-      $ 
Horizontal=�6%   .+�5.      �� -   5         ��Name$ TextBlock_CreatNewText��5.   (   $�� 建立新檔��6H   5/      51      �� 52      
����·�$��25��6.   $��25��6:   �� 53      -   5        $ Btn_OpenOddText��53      $��75��62   $��25��6=   #��[  6[   ��54      �� 55      $ 
Horizontal=�6%   .+�56      �� -   5        $ TextBlock_OpenOddText��56   (   $�� 開啟舊檔��6G   57      59      �� 5:      
����·�$��25��6.   $��25��6:   �� 5;      -   5        $ 
Btn_Cancel��5;      $��75��6-   $��25��68   #��[  6V   ��5<      �� 5=      $ 
Horizontal=�6%   .+�5>      �� -   5        $ TextBlock_Cancel��5>   (   $�� 取消��6B   5?      5A      5B      5C      5D      5E      