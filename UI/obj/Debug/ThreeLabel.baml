   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:UIUI  5      T PEmgu.CV.UI, Version=4.1.1.3497, Culture=neutral, PublicKeyToken=7281126722ab4438;,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI
Emgu.CV.UI 5      [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5          UI.ThreeLabel   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      "localclr-namespace:UI     5      =emgu,clr-namespace:Emgu.CV.UI;assembly=Emgu.CV.UI     5      Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5      $��12(�5
      $��Microsoft JhengHei)�5      $	��280��5      $	��280��6   $��30��6,   $��30��6;     � 	Resources  5         ContextMenu��%!  &	        �� 5   
   .@�5      m� $��MenuItem_Pronounce��6   -   5        $��發音��5   1   6=   m� 5      $��MenuItem_Definition��6   -   5        $��定義��5   2   6>   m� 5      $��MenuItem_Delete��6   -   5        $��刪除��5   .   6:   5      5      .��5      �  � RowDefinitions 5   
   �� 5      -   5         ��Name$ GridRowDef_00��5      $��1 �63   �� 5      -   5        $ GridRowDef_01��5      $��28 �63   �� 5      -   5        $ GridRowDef_02��5      $��1 �63    � ColumnDefinitions 5   
   �� 5      -   5         ��Name$ GridColDef_00��5      $��1 �66   �� 5      -   5        $ GridColDef_01��5      $��92 �66   �� 5      -	   5        $ GridColDef_02��5      $��1 �66   �� 5       -
   5        $ GridColDef_03��5       $��92 �66   �� 5!      -   5        $ GridColDef_04��5!      $��1 �66   �� 5"      -   5        $ GridColDef_05��5"      $��92 �66   �� 5#      -   5        $ GridColDef_06��5#      $��1 �66   .+�5&   
   �� -   5         ��Name$ Boader_English��5&      
����� �$��1��6B   $��1��6R   6_   �� 5'   
   -   5        $ Boader_PartOfSpeech��5'      
����� �$��3��6G   $��1��6W   6d   �� 5(   
   -   5        $ Boader_Chinese��5(      
����� �$��5��6B   $��1��6R   6_   �� 5*   
   -   5         ��Name$ StackPanel_English��5*       �� Orientation$ 
Horizontal=�61   $��Center=�6J   $��1��6g   $��1��6w   .+�5+      �� -   5         ��Name$ Button_English��5+      $�� ��6-   $��Center=�68   $��28��6\   $��92��6h   $��0q�6s   
����� �	 � ContextMenu#	 [  6�   6�   5,      �� 5-   
   -   5        $ StackPanel_PartOfSpeech��5-      $ 
Horizontal=�67   $��Center=�6P   $��3��6m   $��1��6}   .+�5.      �� -   5        
 ��Name$
 Label_PartOfSpeech��5.      $�� ��60   $��Center=�6;   6_   5/      �� 50   
   -   5        $ StackPanel_Chinese��50      $ 
Horizontal=�62   $��Center=�6K   $��5��6h   $��1��6x   .+�51      �� -   5        $
 Label_Chinese��51      $�� ��6+   $��Center=�66   6Z   52      53      54      