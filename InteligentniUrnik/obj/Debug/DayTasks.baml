   M S B A M L   `   `   `  ����L  HInteligentniUrnik, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:InteligentniUrnikInteligentniUrnik  5   	        InteligentniUrnik.DayTasks   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:InteligentniUrnik     5   	     � Title$  DayTasks��5   	   $	��450��6   $	��350��6'    � 
ResizeMode$ NoResize=�63    � AllowsTransparency . 
������  � WindowStyle$
 None=�5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��5* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   ��  �� CornerRadius$ 10��6   $��4��6#   ��5      �� 5      .��5      �  � Color$ #98ec16��6#    � Offset$	 0.0Q�63   � 5      $ #FCBD06��6#   $ 1Q�63   5      5      �� 5   
   -   5         ��Name$ dayMonth��5!      $��35(�5      $��Placeholder��5      $��Center=�5      $��Light"�5      $��	Helvetica)�5      
�������$��0��5"      �� 5$   
   $��1��6   ._�5%      �� -   5        	 ��Name$	 listViewTasks��5%   0   
������ $��0q�6G   ��5'      �� 5(      
 �� 
TargetType"
 ��6   .�5)      ��  �� Property � �� �� Value$ 	Helvetica)�67   �� 5*       � ��
 �����67   �� 5+       � ��$ 15(�65   5,      51      52      �� 54   
   -   5         ��Name$ addTaskToDay��54   �   $��2��6,   $��Assign task to day��69   $��Light"�6V   
�������$��	Helvetica)�6|   $��25(�6�   $��0q�6�   
������ $��3q�6�   � 56   
   $��3��6    � ColumnDefinitions 57      �� 58      $��* �6#   �� 59      $��* �6#   .+�5<      �� -   5        $ OkButton��5<   �   $��0��6,   $��OK��6<   $��Light"�6I   
�������$��	Helvetica)�6o   $��25(�6�   $��0q�6�   
������ $��3q�6�   �� 5=      -   5        $ CancelButton��5=   �    �� IsCancel . $��1��6@   $��Cancel��6P   $��Light"�6a   
�������$��	Helvetica)�6�   $��25(�6�   $��0q�6�   
������ $��3q�6�   5?      5B      5C      