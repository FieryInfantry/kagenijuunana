   M S B A M L   `   `   `  ����L  HStudentElectionV1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:StudentElectionV1StudentElectionV1  5   	   &     StudentElectionV1.UserManagement   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:StudentElectionV1     5   	     � WindowStyle$
  None=�5       � AllowsTransparency$
 True��5   	   
������  � WindowStartupLocation$ CenterScreen=�5	   	    � Title$ User Management��5
   	   $	��900��6!   $
��1100��6.   .��5      �� ���#0f5ebf �� CornerRadius$ 30��6"   ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��800 �6#   �� 5       � RowDefinitions 5      �� 5      $	��700 �6    �� 5      .+�5      �� 
�������$��10q�6(   $ 15��64   5      �� 5      
���   �$��10q�6(   $ 15��5      $��1��5      $��2��6"   ._�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $
��Left=�6!   $��10q�6<   .+�5      �� 
�������
������ $	��120��5   !   $	��120��6-   .��5       �� $��images/add-entry.png��6$   5!      �� 5"      $��18(�6$   $	��Add��62   $��Center=�5#   $   
�������5$   $   5%      �� 5&      $
��Left=�6!   $��10q�6<   .+�5'      �� 
�������
������ $	��120��5)      $	��120��6)   .��5*      �� $��images/update-entry.png��6$   5+      �� 5,      $��18(�6$   $��Update��62   $��Center=�5-      
�������5.      5/      �� 50      $
��Left=�6!   $��10q�6<   .+�51      �� 
�������
������ $	��120��53      $	��120��6)   .��54      �� $��images/delete-entry.png��6$   55      �� 56      $��18(�6$   $��Delete��62   $��Center=�57      
�������58      59      �� 5:      $��500 10 10 10q�6!   .+�5;      �� 
�������
������ $	��120��5=      $	��120��6)   .��5>      �� $��images/exit.png��6$   5?      �� 5@      $��18(�6$   $��Close��62   $��Center=�5A      
�������5B      5C      5D      5E      �� 5F      $��1��6   $��/images/student.png��6$   5G      5H      5I      