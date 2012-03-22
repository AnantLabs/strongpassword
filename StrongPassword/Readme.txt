                                                                          
                                                                          
       .-.  .                     .--.                                  . 
      (   )_|_                    |   )                                 | 
       `-.  |  .--..-. .--. .-..  |--'.-.  .--..--..  .    ._.-. .--..-.| 
      (   ) |  |  (   )|  |(   |  |  (   ) `--.`--. \  \  / (   )|  (   | 
       `-'  `-''   `-' '  `-`-`|  '   `-'`-`--'`--'  `' `'   `-' '   `-'`-
                            ._.'                                          
												Version: 2.0.0 Open Beta
--------------------------------------------------------------------------------
Was is it?
--------------------------------------------------------------------------------
It´s a simple program to make a keyword into a strong password.

Filelocations:
%USERPROFILE%\AppData\Local\StrongPassword\Profile.xml	- All custom profiles
%USERPROFILE%\AppData\Local\StrongPassword\Settings.xml	- Master Keyword

--------------------------------------------------------------------------------
Sourcecode Notes
--------------------------------------------------------------------------------
BCrypt				- C/C++ class library	- BCrypt hashalgorithm from OpenBSD sourcecode
CryptWrapper		- C# class library		- Wrapper to the BCrypt project, a nice place to store the pinvoke
StrongPassword		- C# executable			- GUI

BCrypt note - This product includes software developed by Niels Provos. 
Proud fan of ReSharper!
This software comes with NO WARRANTY WHATSOEVER. You may use this software at your own risk. 
I can take no responsibility for any damage caused by this application. 

--
Erik Beijer
erik20beijer@gmail.com