   M S B A M L   `   `   `  ����D  @WPFClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WPFClient.PagesWPFClient.Pages  5      "    WPFClient.Pages.InvoicesPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WPFClient.Pages     5        N� Title$  InvoicesPage��5	      .��5      � ���#fff � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��3 �6   �� 5      -   5         ��Name$ DialogFrame��5      $��0 �64   $	��400��6>   .+�5   
   �� $��
Счета��6   $��2��6    $��26(�64   $��	ExtraBold"�6B   �� 5   
   $��1��6   $��0��6"   $��3��62    �� Orientation$ 
Horizontal=�6F   .+�5      �� -   5         ��Name$ 	AddButton��5      $��Добавить��6(   $��30��6;   $	��100��6G   $��5,5,5,5q�6S   $��5,5,0,5q�6e   6v   �� 5      -   5        $ 
CopyButton��5      $��Копировать��6)   $��30��6>   $	��100��6J   $��5,5,5,5q�6V   $��5,5,0,5q�6h   6y   �� 5      -   5        $ 
EditButton��5      $��Изменить��6)   $��30��6<   $	��100��6H   $��5,5,5,5q�6T   $��5,5,0,5q�6f   6w   �� 5      -   5        $ DeleteButton��5      $��Удалить��6+   $��30��6=   $	��100��6I   $��5,5,5,5q�6U   $��5,5,0,5q�6g   6x   �� 5      $��|��6   $��24(�6    $
��Bold"�6.   
�������$��15,0,0,0q�6R   �� 5       -   5         ��Name$ FilterComboBox��5       $	��150��6/   $��30��6;   $��15,0,0,0q�6G   �� 5!      -   5         ��Name$ FilterTextBox��5!      $��5,0,5,0q�6-   $	��170��6>   $��30��6J   5"      ]� 5$   
   $��2��6   $��2��6!   .+�5%      &   System.Windows.Controls.DataGrid  -   5          Name$ InvoicesGrid��5%      	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     
IsReadOnly .    Columns 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Header$ Settled��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   ��  �� Path$ Settled�  5(      $ Яхта��6)    67   �� $ Contract.Order.Boat.Model�  5)      $ 
Сумма��6)    68   �� $	 Sum�  5*      $ 
Налог��6)    68   �� $ Sum_inclVAT�  5+      $ Дата��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6U    67   �� $
 Date�6U   5-      5.      �� 50   
   $��1��6   $��2��6'   $��3��64   $��Stretch=�6>   �� 52   
   $��2��6   $��2��6"   $ Vertical=�62   .+�53      �� -	   5         ��Name$ DialogTitle��53      $��Добавить счет��6)   $��16(�6A   $
��Bold"�6O   $��5,0,0,0q�6a   �� 55      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�56      �� $��Settled��6   $��5,0,0,0q�6*   $	��130��6;   �� 57      -
   5        $ Settled��57      $	��150��6+   $��25��67   $��5,0,0,0q�6C   ��6T   ��  �� ElementName$ InvoicesGrid��$ SelectedItem.Settled�58      �� 5:      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5;      �� $��Яхта��6   $��5,0,0,0q�6'   $	��130��68   �� 5<      -   5        $
 Boat��5<      $	��150��6)   $��25��65   $��5,0,0,0q�6A    �� DisplayMemberPath$ Model��6R    �� Text 6l   �� $ InvoicesGrid��$, &SelectedItem.Contract.Order.Boat.Model�5=      �� 5?      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5@      �� $��
Сумма��6   $��5,0,0,0q�6(   $	��130��69   �� 5A      -   5        $	 Sum��5A      $	��150��6'   $��25��63   $��5,0,0,0q�6?   ��6P   �� $ InvoicesGrid��$ SelectedItem.Sum�5B      �� 5D      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5E      �� $��
Налог��6   $��5,0,0,0q�6(   $	��130��69   �� 5F      -   5        $ 
SumInclVAT��5F      $	��150��6.   $��25��6:   $��5,0,0,0q�6F   ��6W   �� $ InvoicesGrid��$ SelectedItem.Sum_inclVAT�5G      �� 5I      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5J      �� $��Дата��6   $��5,0,0,0q�6'   $	��130��68   �� 5K      -   5        $
 Date��5K      $	��150��6(   $��25��64   $��5,0,0,0q�6@   ��6Q   �� $ InvoicesGrid��$ SelectedItem.Date�5L      �� 5N      $ 
Horizontal=�6   $��Center=�62   $��0,15,0,0q�6O   .+�5O      �� -   5        $ ApplyButton��5O      $��Добавить счет��6.   $��30��6F   $	��100��6R   $��5,5,5,5q�6a   $��5,5,0,5q�6s   6�   �� 5P      -   5        $ CancelButton��5P      $��Отменить��6/   $��30��6B   $	��100��6N   $��5,5,5,5q�6Z   $��5,5,0,5q�6l   6}   5Q      5R      5S      5T      