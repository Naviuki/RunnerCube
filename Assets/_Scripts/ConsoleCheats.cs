   using UnityEngine;
   using UnityEngine.UI;
   using System.Collections;
   using System.Collections.Generic;

   public class ConsoleCheats : MonoBehaviour
   {
       public InputField inputField;
       public Text consoleOutput;
       private PlayerController playerController;
       public CoinText coinText;

       void Start()
       {
           playerController = FindObjectOfType<PlayerController>();
           inputField.onEndEdit.AddListener(ProcessCommand);
       }

       void Update()
       {
            if(!playerController.isHardcore)
            {
                RenderSettings.fogDensity = 0.005f;
            }
       }

       IEnumerator ExitGameCoroutine()
       {
            yield return new WaitForSeconds(1);
            consoleOutput.text += "\n3";
            yield return new WaitForSeconds(1);
            consoleOutput.text += "\n2";
            yield return new WaitForSeconds(1);
            consoleOutput.text += "\n1";
            Application.Quit();
       }

       void ProcessCommand(string command)
       {
           if (string.IsNullOrEmpty(command)) return;

           string[] args = command.Split(' ');
           if (args.Length == 0) return;

           switch (args[0].ToUpper())
           {
               case "SPEED":
                   if (args.Length > 1 && float.TryParse(args[1], out float speed))
                   {
                        if(speed < 1000)
                        {
                            playerController.SetSpeed(speed);
                            consoleOutput.text += $"\nSpeed set to {speed}";
                        }
                        else
                        {
                            playerController.speed += 0;
                            consoleOutput.text += $"\nBe careful with this setting. Operation canceled!";
                        }
                       
                   }
                   else
                   {
                       consoleOutput.text += "\nInvalid command format. Use: SPEED <value>";
                   }
                   break;

                case "JUMP":
                    if (args.Length > 1 && float.TryParse(args[1], out float jumpForce))
                    {
                        if(jumpForce < 100)
                        {
                            playerController.SetJumpForce(jumpForce);
                            consoleOutput.text += $"\nJump force set to {jumpForce}";
                        }
                        else
                        {
                            playerController.speed += 0;
                            consoleOutput.text += $"\nBe careful with this setting. You are not a frog. \nComeback when you became the frog";
                        }
 
                    }
                    else
                    {
                        consoleOutput.text += "\nInvalid command format. Use: JUMP <value>";
                    }
                    break;

                case "COINS":
                    if (args.Length > 1 && int.TryParse(args[1], out int coins))
                    {
                        if(coins <= 0)
                        {
                            consoleOutput.text += $"\nYou cant do it!. Yeah, you just cant";
                            coins = 0;
                        }
                        else
                        {
                            coinText.SetCoins(coins);
                            consoleOutput.text += $"\nCoins set to {coins}";
                        }

                    }
                    else
                    {
                        consoleOutput.text += "\nInvalid command format. Use: GIVE COINS <value>";
                    }
                    break;

                case "HARDCORE":
                    if(args.Length > 1 && bool.TryParse(args[1], out bool isHardcore))
                    {
                        if (playerController.isImmortal == false)
                        {
                            playerController.SetHardcore(isHardcore);
                            consoleOutput.text += $"\nHARDCORE mode set to {isHardcore}";
                        }
                    }

                   else
                   {
                       consoleOutput.text += "\nInvalid command format. Use: SET_HARDCORE <true/false>";
                   }
                   if(playerController.isImmortal == true)
                   {
                        consoleOutput.text += $"\nFirst disable IMMORTAL mode";
                   }
                   break;

               case "IMMORTAL":
                   if (args.Length > 1 && bool.TryParse(args[1], out bool isImmortal))
                   {
                        if(!playerController.isHardcore)
                        {
                            playerController.SetImmortal(isImmortal);
                            consoleOutput.text += $"\nIMMORTAL mode set to {isImmortal}";
                        }
                        else
                        {
                            consoleOutput.text += $"\nYou are not GENIUS ";
                            consoleOutput.text += "\nThe game will close in 3 seconds";
                            StartCoroutine(ExitGameCoroutine());
                            Application.Quit();
                        }  
                   }
                   else
                   {
                       consoleOutput.text += "\nInvalid command format. Use: GAMEMODE IMMORTAL <true/false>";
                   }
                   break;
                case "HELP":
                    consoleOutput.text += "\nAvailable commands:";
                    consoleOutput.text += $"\nSpeed  <value> - set the entered speed value";
                    consoleOutput.text += $"\nJump  <value> - set the entered jump value";
                    consoleOutput.text += $"\nCoins  <value> - set the entered coins value";
                    consoleOutput.text += $"\nHardcore  <true/false> - on/off HARDCORE mode";
                    consoleOutput.text += $"\nImmortal  <true/false> - on/off IMMORTAL mode";
                    break;

               default:
                   consoleOutput.text += "\nUnknown command. Use HELP to see the list of available commands.";
                   break;
           }

           inputField.text = "";
       }
   }
   
