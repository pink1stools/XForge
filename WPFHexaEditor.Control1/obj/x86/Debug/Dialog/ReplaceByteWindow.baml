   M S B A M L   `   `   `  ????H  DWPFHexaEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:WPFHexaEditor.Control.DialogWPFHexaEditor.Control.Dialog  5   	   =#clr-namespace:WPFHexaEditor.ControlWPFHexaEditor.Control  5   	   4   .WPFHexaEditor.Control.Dialog.ReplaceByteWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:WPFHexaEditor.Control.Dialog     5   	   7Control#clr-namespace:WPFHexaEditor.Control     5   	   -   5        
???????$??168.968??5   	   $	??431??6     ? WindowStartupLocation$  CenterOwner=?5   	    ? WindowStyle$ SingleBorderWindow=?5   	    ? 
ResizeMode$ NoResize=?5   	    ? ShowInTaskbar .  .??5      ? .+?5   
   ?? -   5         ??Name$ OKButton??5      $??OK??5      $??2??5       ?? 	IsDefault . $??Right=?5      $??Bottom=?5      $??75??5      $??	0,0,92,13q?5      5      ?? 5   
   $??Cancel??6   $??3??5       ?? IsCancel . $??	0,0,12,13q?5       $??20??5!      $??Bottom=?5"      $??Right=?5#      $??75??5$      ?? 5&   
   $??	Find byte??6   $
??Left=?5'      $??	10,18,0,0q?5(      $	??Top=?5)      "   WPFHexaEditor.Control.HexBox  5+   
   -   5          Name$ 
HexTextBox??5,      $??23??5-      $??126,19,50,0q?5.      $	??Top=?5/      $
??Auto??50         MaximumValue$	 255??6   ?? 52   
   $??Replace by byte??6   $
??Left=?53      $??	10,60,0,0q?54      $	??Top=?55        57   
   -   5        $ ReplaceHexTextBox??58      $??23??59      $??126,61,50,0q?5:      $	??Top=?5;      $
??Auto??5<      $	 255??6   5=      5>      