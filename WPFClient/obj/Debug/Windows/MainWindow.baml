   M S B A M L   `   `   `  ����D  @WPFClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WPFClient	WPFClient  5   	       WPFClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:WPFClient     5   	     � Title$  
WPF Client��5   	   $
��1024��5	   	   $	��576��6   $	��400��5
   	   $	��225��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��125 �6   $	��125��6+   $	��200��6:   �� 5      $
��auto �6   �� 5      $	��100��6    � RowDefinitions 5   
   �� 5      �� 5      $��30 �6   .+�5   
   �� $��2��6      PrimaryColor#��[  6&   .+�5      �� $��/Images/yacht.png��6   $��64��6/   $��64��6:   �� 5      -   5         ��Name$ 	RoleLabel��5      $ ��Администратор��6'   $��Center=�6?   $
��Bold"�6\   $��10(�6n   �� 5      -   5         ��Name$ BoatsButton��5      $��Яхты��6*   69   �� 5      -   5        $ AccessoriesButton��5      $��Аксессуары��60   6E   �� 5      -   5        $ ConstractsButton��5      $��Контракты��6/   6C   �� 5      -   5        $ AccessoriesToBoatButton��5      $(��"Аксессуары к яхтам��66   6S   �� 5      -   5        $ InvoicesButton��5      $��
Счета��6-   6=   �� 5      -   5        $ OrdersButton��5      $��Заказы��6+   6<   �� 5       -   5        $ OrderDetailsButton��5       $!��Детали заказов��61   6J   �� 5!      -	   5        $ CustomersButton��5!      $��Покупатели��6.   6C   �� 5"      -
   5        $ SalePersonssButton��5"      $��Продавцы��61   6D   �� 5#      -   5        $ PartnersButton��5#      $��Партнеры��6-   6@   5$      �� 5&   
   $��1��6   $��0��6'   $��2��64   $��2��6E   $��Stretch=�6O   #��[  6m   �� 5'   
   $��2��6   $��1��6$   #��[  61   �� 5)   
   $��2��6   $��1��6%    �� Orientation$ 
Horizontal=�62   .+�5*      �� -   5        $ 
BackButton��5*      $��
Назад��6)     NavigationButtons#��[ 69   �� 5+      -   5        $ 
NextButton��5+      $��Вперед��6)   #��[ 6:   5,      �� 5.   
   $��2��6   $��1��6%   $ 
Horizontal=�62   $��Right=�6K   .+�5/      �� -   5        $ CloseButton��5/      $%��Закрыть страницу��6*   $	��125��6E   #��[ 6Q   50       � 52   
   -   5          �Name$ 	RootFrame��52       $��2��6     � NavigationUIVisibility$ Hidden=�63   53      54      