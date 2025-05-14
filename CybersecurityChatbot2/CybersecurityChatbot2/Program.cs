using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;

public class CybersecurityChatbot2
{

    private Dictionary<string, string> _userInfo = new Dictionary<string, string>();


    private List<string> _conversationHistory = new List<string>();


    private Random _random = new Random();

 
    private const string DefaultResponse = "I'm not sure how to answer that yet. Try asking about malware, phishing, or password safety.";

  
    private Dictionary<string, string> _sentimentResponses = new Dictionary<string, string>
    {
        { "worried", "It's okay to feel that way. Cybersecurity can seem complex, but I'm here to guide you step by step." },
        { "nervous", "It's okay to feel that way. Cybersecurity can seem complex, but I'm here to guide you step by step." },
        { "frustrated", "Don't worry. Everyone starts somewhere." },
        { "confused", "Don't worry. Everyone starts somewhere." },
        { "curious", "That's great! Curiosity is the first step toward cyber awareness." },
        { "interested", "That's great! Curiosity is the first step toward cyber awareness." }
    };


    private List<string> _privacyKeywords = new List<string> { "privacy", "personal data", "data protection", "settings" };
    private List<string> _phishingKeywords = new List<string> { "phishing", "suspicious email", "fake website", "scam" };
    private List<string> _malwareKeywords = new List<string> { "malware", "virus", "trojan", "spyware" };
    private List<string> _passwordKeywords = new List<string> { "password", "strong password", "passwords", "credentials" };

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CybersecurityChatbot2 chatbot = new CybersecurityChatbot2();

      
        try
        {
            using (SoundPlayer player = new SoundPlayer("..\\..\\welcome.wav"))
            {
                player.Play();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error playing welcome sound: " + ex.Message);
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@"
   ____          _                       _
  / ___|  ___| |_ _  _ _ __ ___| |__   ___ _ __  _  _
  \___ \ / _ \ __| | | | '__/ __| '_ \ / _ \ '_ \| | | |
   ___) | __/ |_| |_| | | | (__| | | | __/ | | | |_| |
  |____/ \___|\__|\__,_|_|  \___|_| |_|\___|_| |_|\__,_|

      Welcome to the Cybersecurity Chatbot ( PROGRAMMING 2A - PROG6221 )
                 Created by: Hlompho PETJA (part one and two)
        ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Enter your name: ");
        Console.ResetColor();
        string userName = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Name cannot be empty.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter your name: ");
            Console.ResetColor();
            userName = Console.ReadLine();
        }

        chatbot._userInfo["name"] = userName; 
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nWelcome, {userName}! Let’s explore the world of cybersecurity together.");
        Console.ResetColor();

     
        chatbot.ChatbotInteraction();
    }

   
    private void ChatbotInteraction()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Ask a question or type 'exit' to quit: ");
            Console.ResetColor();
            string input = Console.ReadLine().ToLower();
            _conversationHistory.Add("You: " + input); 

            if (input == "exit")
            {
                Respond("Goodbye!");
                Thread.Sleep(50);
                break;
            }

        
            string sentimentResponse = DetectSentiment(input);
            if (sentimentResponse != null)
            {
                Respond(sentimentResponse);
                continue; 
            }

            
            switch (input)
            {
                case "how are you?":
                case "how are you":
                    Respond("I’m running smoothly, thanks for asking! Ready to help you stay cyber safe.");
                    continue;
                case "what’s your purpose?":
                case "what is your purpose?":
                case "why are you here?":
                    Respond("My mission is to help you understand and stay safe in the digital world! Ask me anything about cybersecurity.");
                    continue;
                case "what can i ask you about?":
                case "what do you know?":
                case "help":
                    Respond("You can ask me about:\n" +
                            " Password Safety\n" +
                            " Phishing Scams\n" +
                            "Safe Browsing Habits\n" +
                            " Malware and how to avoid it\n" +
                            "Mobile device security\n" +
                            "Online privacy tips\n" +
                            "Try asking something like 'How do I protect my password?' or 'What is phishing?'");
                    continue;
                case "what is phishing?":
                    Respond("Phishing is a cyber scam where attackers pretend to be trustworthy to trick you into giving personal info.");
                    continue;
                case "how do i protect my password?":
                    Respond("Use long, complex passwords with symbols, numbers, and upper/lowercase letters. And don't reuse passwords!");
                    continue;
                case "how do i browse safely?":
                case "how to stay safe online?":
                    Respond("Stick to trusted websites, avoid clicking unknown links, and always update your browser and antivirus software.");
                    continue;
                case "what is malware?":
                    Respond("Malware is software designed to damage or gain unauthorized access to your system.");
                    continue;
            }

            string advancedResponse = GenerateAdvancedResponse(input);
            Respond(advancedResponse);
        }
    }

 
    public string GenerateAdvancedResponse(string userInput)
    {
      
        if (string.IsNullOrEmpty(userInput))
        {
            return DefaultResponse;
        }

      
        string lowerInput = userInput.ToLower();

       
        if (ContainsKeywords(lowerInput, _privacyKeywords))
        {
            return GetPrivacyResponse(lowerInput);
        }
        else if (ContainsKeywords(lowerInput, _phishingKeywords))
        {
            return GetPhishingResponse(lowerInput);
        }
        else if (ContainsKeywords(lowerInput, _malwareKeywords))
        {
            return GetMalwareResponse(lowerInput);
        }
        else if (ContainsKeywords(lowerInput, _passwordKeywords))
        {
            return GetPasswordResponse(lowerInput);
        }

       
        if (_conversationHistory.Count > 2) 
        {
            string previousResponse = _conversationHistory[_conversationHistory.Count - 2]; 
            if (previousResponse.Contains("review your security settings"))
            {
                return "Have you checked your security settings yet?  It's important to ensure your data is protected.";
            }
            else if (previousResponse.Contains("Be cautious of suspicious emails"))
            {
                return "Did you receive any suspicious emails recently?";
            }
        }

   
        if (lowerInput.Contains("what is my name") && _userInfo.ContainsKey("name"))
        {
            return $"Your name is {_userInfo["name"]}.";
        }

      
     
        return DefaultResponse;
    }


    private bool ContainsKeywords(string input, List<string> keywords)
    {
        return keywords.Any(keyword => input.Contains(keyword));
    }


    private string ExtractName(string input)
    {
        Match match = Regex.Match(input, @"my name is\s+(?<name>\w+)");
        if (match.Success)
        {
            return match.Groups["name"].Value;
        }
        return null;
    }

   
    private string DetectSentiment(string input)
    {
        foreach (var kvp in _sentimentResponses)
        {
            if (input.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return null; 
    }

   
    private string GetPrivacyResponse(string input)
    {
        string[] responses = {
            "It's important to protect your privacy online. I recommend you review your security settings.",
            "Are you concerned about a specific privacy issue? I can help you find relevant information.",
            "Do you know how your data is being used?  I can explain data protection policies."
        };
        int index = _random.Next(responses.Length);
        return responses[index];
    }

    private string GetPhishingResponse(string input)
    {
        string[] responses = {
            "Be cautious of suspicious emails and websites. Do not click on unknown links.",
            "Phishing is a common attack.  I can help you identify phishing attempts.",
            "Have you encountered a potential phishing situation?  Describe it to me."
        };
        int index = _random.Next(responses.Length);
        return responses[index];
    }

    private string GetMalwareResponse(string input)
    {
        string[] responses = {
            "Malware can seriously harm your computer.  Make sure you have up-to-date antivirus software.",
            "Are you worried about a specific type of malware?",
            "I can provide information on how to protect yourself from malware."
        };
        int index = _random.Next(responses.Length);
        return responses[index];
    }

    private string GetPasswordResponse(string input)
    {
        string[] responses = {
            "Use a strong, unique password for each of your accounts.",
            "Password managers can help you create and store strong passwords.",
            "Do you need help creating a strong password?"
        };
        int index = _random.Next(responses.Length);
        return responses[index];
    }

 
    static void Respond(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < message.Length; i++)
        {
            Console.Write(message[i]);
            Thread.Sleep(25);
        }
        Console.WriteLine();
        Console.ResetColor();
    }
}