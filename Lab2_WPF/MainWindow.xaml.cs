using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2_WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SpeechSynthesizer ss;
        static SpeechRecognitionEngine sre;
        static bool stop;

        public MainWindow()
        {
            InitializeComponent();

            stop = false;
            ss = new SpeechSynthesizer();
            ss.SetOutputToDefaultAudioDevice();
            ss.SpeakAsync("Witam w laboratoerium dwa");
            CultureInfo ci2 = new CultureInfo("pl-PL");
            sre = new SpeechRecognitionEngine(ci2);
            sre.SpeechRecognized += Sre_SpeechRecognized;

            Grammar grammar = new Grammar(".\\Grammars\\Grammar1.xml");
            grammar.Enabled = true;
            sre.SetInputToDefaultAudioDevice();
            sre.LoadGrammar(grammar);
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence;
            int result;
            if (confidence < 0.6)
            {
                ss.Speak("Zbyt ma ła pewność");
            }
            else
            {
                string operation = e.Result.Semantics["operation"].Value.ToString();
                int num1 = Convert.ToInt32(e.Result.Semantics["first"].Value);
                int num2 = Convert.ToInt32(e.Result.Semantics["second"].Value);
                if (operation == "plus")
                {
                    result = num1 + num2;
                    WYNIK.Text = num1 + " + " + num2 + " = " + result;
                    if (result == 0)
                        ss.SpeakAsync("Wynik twojego dodawania to zero");
                    else
                        ss.SpeakAsync("Wynik twojego dodawania to " + result);
                }
                else if (operation == "minus")
                {
                    result = num1 - num2;
                    WYNIK.Text = num1 + " - " + num2 + " = " + result;
                    if (result > 0)
                        ss.SpeakAsync("Wynik twojego odejmowania to " + result);
                    else if (result == 0)
                        ss.SpeakAsync("Wynik twojego odejmowania to zero");
                    else
                        ss.SpeakAsync("Wynik twojego odejmowania to minus " + result);

                }
                else if (operation == "multiply")
                {
                    result = num1 * num2;
                    WYNIK.Text = num1 + " x " + num2 + " = " + result;
                    if (result == 0)
                        ss.SpeakAsync("Wynik twojego mnożenia to zero");
                    else
                        ss.SpeakAsync("Wynik twojego mnożenia  to " + result);

                }
                else if (operation == "divide")
                {
                    if (num2 == 0)
                    {
                        ss.SpeakAsync("Nie można dzielić przez zero");
                    }
                    else
                    {
                        result = num1 / num2;
                        WYNIK.Text = num1 + " / " + num2 + " = " + result;
                        if (result == 0)
                            ss.SpeakAsync("Wynik twojego dzielenia to zero");
                        else
                            ss.SpeakAsync("Wynik twojego dzielenia to " + result);
                    }
                }
            }
        }
    }
}
