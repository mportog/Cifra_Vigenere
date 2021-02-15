using System;
using System.Collections.Generic;
using System.Linq;

namespace QuebrandoCifraVinegere
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cole o texto cifrado com criptografia de Vinerege, para encontrar a chave:");
            string textoCifrado = Console.ReadLine();

            var frequencia = MaiorFrequenciaChar(textoCifrado);
            foreach (var item in frequencia)
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }

            var maxChar = frequencia.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            Console.WriteLine(string.Format("CHAR de maior frequencia: {0}", maxChar));

            // Define quantos digitos para encontrar padrões na sequencia
            int tamChave;
            string tamChaveString;
            do
            {
                //geralmente varia de 3 a 4
                Console.WriteLine("Digite o tamanho da separação da sequencia:");
                tamChaveString = Console.ReadLine();

            } while (!int.TryParse(tamChaveString, out tamChave));

            var ocorrencia = Ocorrencia(textoCifrado, tamChave);
            var sequencia = ocorrencia.Item1;
            var periodicidade = ocorrencia.Item2;

            Console.WriteLine(string.Format("Maior frequência de aparição DA SEQUÊNCIA de {0} chars e menor distancia entre ocorrencias", tamChave));
            foreach (var item in sequencia)
            {
                Console.WriteLine(string.Format("Chave: {0}", item.Key));
                Console.WriteLine(string.Format("aparições: {0}", item.Value));
                Console.WriteLine(string.Format("menor distância: {0} \n", periodicidade[item.Key].Distancia));
            }
            var maiorA = sequencia.Values.Where(v => v > 2).Max();
            var menorD = periodicidade.Values.Where(v => v.Distancia > 0 && v.Ocorrenca>2).Min(m => m.Distancia);
            Console.WriteLine(string.Format("Maior aparição: {0} vezes", maiorA));
            Console.WriteLine(string.Format("Menor distancia: separado por {0} chars", menorD));


            // encontrar o tamanho da CHAVE, dividir o texto todo cifrado em colunas, do tamanho da CAHVE
            Console.WriteLine("--- Assumindo que o char mais recorrente seja o \" \" (espaço em branco) ---");
            // b=" ";


            // listar ocorrencias similares das letras e ordenar ocorrencias das colunas para encontrar as letras da chave, uma a uma
        }

        /// <summary>
        /// alfabeto em ASCII utilizado para cifrar a mensagem
        /// </summary>
        readonly List<char> alfabeto = new List<char> {
            '!','"','#','$','%','&','(',')','*','+',',','-','.','/',
            '0','1','2','3','4','5','6','7','8','9',
            ':',';','<','=','>','?','@',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            '[','\\',']','^','_','`',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        };

        /// <summary>
        /// Busca ocorrencia da sequencia
        /// </summary>
        /// <param name="textoCifrado">Mensagem codificada pela cifra de Venegere</param>
        /// <param name="tamChave">Tamanho da sequencia de caracteres a serem comparadas</param>
        /// <returns>Dicionários com aparições, ocorrencias e distancias</returns>
        public static (Dictionary<string, int>, Dictionary<string, Ultima>) Ocorrencia(string textoCifrado, int tamChave)
        {
            Dictionary<string, int> sequencia = new Dictionary<string, int>();
            Dictionary<string, Ultima> periodicidade = new Dictionary<string, Ultima>();

            for (int i = 0; i < (textoCifrado.Length - tamChave); i++)
            {
                var seq = textoCifrado.Substring(i, tamChave);

                if (sequencia.ContainsKey(seq))
                {
                    int incrementar = sequencia.GetValueOrDefault(seq);
                    sequencia[seq] = incrementar + 1;

                    Ultima ultimaVez = periodicidade[seq];
                    if (i - ultimaVez.Ocorrenca < ultimaVez.Distancia || ultimaVez.Distancia == 0)
                    {
                        int d = i - ultimaVez.Ocorrenca;
                        periodicidade[seq] = new Ultima()
                        {
                            Distancia = d,
                            Ocorrenca = i
                        };
                    }
                }
                else
                {
                    sequencia.Add(seq, 1);

                    Ultima primeira = new Ultima()
                    {
                        Distancia = 0,
                        Ocorrenca = i
                    };

                    periodicidade.Add(seq, primeira);
                }
            }

            return (sequencia, periodicidade);
        }

        /// <summary>
        /// Busca cada char utilizado e conta quantas aparições teve
        /// </summary>
        /// <param name="textoCifrado"></param>
        /// <returns>Dicionário de chars e a quantidade de aparições</returns>
        public static Dictionary<char, int> MaiorFrequenciaChar(string textoCifrado)
        {
            Dictionary<char, int> frequencia = new Dictionary<char, int>();

            for (int i = 0; i < textoCifrado.Length; i++)
            {
                if (frequencia.ContainsKey(textoCifrado[i]))
                {
                    int incrementar = frequencia.GetValueOrDefault(textoCifrado[i]);
                    frequencia[textoCifrado[i]] = incrementar + 1;
                }
                else
                {
                    frequencia.Add(textoCifrado[i], 1);
                }
            }
            Console.WriteLine("Maior frequência de aparição dos CHARS");
            return frequencia;
        }
    }

    /// <summary>
    /// Classe que armazena dados de repetições
    /// </summary>
    /// <remarks>Ocorrencia corresponde ao último índice de aparição
    /// Distância corresponde ao menor intervalo de aparição</remarks>        
    public class Ultima
    {
        public int Ocorrenca { get; set; }
        public int Distancia { get; set; }
    }
}


/*var maxSeq = sequencia.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
              Console.WriteLine(string.Format("SEQUENCIA de maior frequencia: {0}", maxSeq));
                // encontrar a distancia nas repetições -> nos diz o tamanho da chave que vai se repetir
                 List<int> periodicidade = new List<int>();
                 sequencia.Clear();
                 for (int i = 0; i < (textoCifrado.Length - tamChave); i++)
                 {
                     var seq = textoCifrado.Substring(i, tamChave);

                     if (sequencia.ContainsKey(seq))
                     {
                         int incrementar = sequencia.GetValueOrDefault(seq);
                         sequencia[seq] = incrementar + 1;
                         if (seq == maxSeq)
                         {
                             periodicidade.Add(i);
                         }
                     }
                     else
                     {
                         sequencia.Add(seq, 1);
                     }
                 }       

                 Console.WriteLine(string.Format("Periodicidade da sequência: {0}", maxSeq));
                 foreach (var item in periodicidade)
                 {
                     Console.WriteLine(string.Format("Ocorre em: {0}",item));
                 }
      */
