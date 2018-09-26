﻿using BitBotBackToTheFuture;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


enum TendencyMarket
{
    HIGH,
    NORMAL,
    LOW,
    VERY_LOW,
    VERY_HIGH
}

class MainClass
{


    //REAL NET
    public static string version = "0.0.0.11";
    public static string location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\";

    public static string bitmexKey = "";
    public static string bitmexSecret = "";
    public static string bitmexKeyWeb = "";
    public static string bitmexSecretWeb = "";
    public static string timeGraph = "";
    public static string statusShort = "";
    public static string statusLong = "";
    public static string pair = "";
    public static int qtdyContacts = 0;
    public static int interval = 0;
    public static int intervalOrder = 0;
    public static int intervalCapture = 0;
    public static int intervalCancelOrder = 30;
    public static int positionContracts = 0;
    public static double profit = 0;
    public static int limiteOrder = 0;
    public static double fee = 0;
    public static double stoploss = 10;
    public static double stopgain = 15;
    public static string bitmexDomain = "";
    public static bool roeAutomatic = true;
    public static bool usedb = false;
    public static bool scalper = true;
    public static double roe = 0;
    public static double stepValue = 0.5;
    public static TendencyMarket tendencyMarket = TendencyMarket.NORMAL;
    public static BitMEX.BitMEXApi bitMEXApi = null;

    public static List<IIndicator> lstIndicatorsAll = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntry = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntryCross = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntryDecision = new List<IIndicator>();

    public static double[] arrayPriceClose = new double[100];
    public static double[] arrayPriceHigh = new double[100];
    public static double[] arrayPriceLow = new double[100];
    public static double[] arrayPriceVolume = new double[100];
    public static double[] arrayPriceOpen = new double[100];

    private static JsonParse configJson = new JsonParse();

    public static Object data = new Object();

    public static void Main(string[] args)
    {
        try
        {
            //Config
            Console.Title = "Loading...";

            Console.ForegroundColor = ConsoleColor.White;

            log("Deleron - Back to the future - v" + version + " - Bitmex version");
            log("by Matheus Grijo ", ConsoleColor.Green);
            log(" ======= HALL OF FAME BOTMEX  ======= ");
            log(" - Lucas Sousa", ConsoleColor.Magenta);
            log(" - Carlos Morato", ConsoleColor.Magenta);
            log(" - Luis Felipe Alves", ConsoleColor.Magenta);
            log(" ======= END HALL OF FAME BOTMEX  ======= ");

            log("http://botmex.ninja/");
            log("GITHUB http://github.com/matheusgrijo", ConsoleColor.Blue);
            log(" ******* DONATE ********* ");
            log("BTC 39DWjHHGXJh9q82ZrxkA8fiZoE37wL8jgh");
            log("BCH qqzwkd4klrfafwvl7ru7p7wpyt5z3sjk6y909xq0qk");
            log("ETH 0x3017E79f460023435ccD285Ff30Bd10834D20777");
            log("ETC 0x088E7E67af94293DB55D61c7B55E2B098d2258D9");
            log("LTC MVT8fxU4WBzdfH5XgvRPWkp7pE4UyzG9G5");
            log("Load config...");
            log("Considere DOAR para o projeto!", ConsoleColor.Green);
            log("Vamos aguardar 10 min para voce doar ;) ... ", ConsoleColor.Blue);
            log("ATENCAO, PARA FACILITAR A DOACAO DAQUI A 30 SEGUNDOS VAMOS ABRIR UMA PAGINA PARA VOCE!", ConsoleColor.Green);
            System.Threading.Thread.Sleep(30000);
            System.Diagnostics.Process.Start("https://www.blockchain.com/btc/payment_request?address=1AnttTLGhzJsX7T96SutWS4N9wPYuBThu8&amount_local=30&currency=USD&nosavecurrency=true");
            log("Perfeito, agora aguarde os 9 minutos e 30 segundos restantes para iniciar o BOTMEX, enquanto isto estamos carregando as suas configuracoes...", ConsoleColor.Magenta);
            String jsonConfig = System.IO.File.ReadAllText(location + "key.txt");
            JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(jsonConfig, (typeof(JContainer)));


            System.Threading.Thread.Sleep(60000 * 10);


            usedb = jCointaner["usedb"].ToString() == "enable";
            scalper = jCointaner["scalper"].ToString() == "enable";
            bitmexKey = jCointaner["key"].ToString();
            bitmexSecret = jCointaner["secret"].ToString();
            bitmexKeyWeb = jCointaner["webserverKey"].ToString();
            bitmexSecretWeb = jCointaner["webserverSecret"].ToString();
            bitmexDomain = jCointaner["domain"].ToString();
            statusShort = jCointaner["short"].ToString();
            statusLong = jCointaner["long"].ToString();
            pair = jCointaner["pair"].ToString();
            ClassDB.strConn = jCointaner["dbcon"].ToString();
            ClassDB.dbquery = jCointaner["dbquery"].ToString();
            timeGraph = jCointaner["timeGraph"].ToString();
            qtdyContacts = int.Parse(jCointaner["contract"].ToString());
            interval = int.Parse(jCointaner["interval"].ToString());
            intervalCancelOrder = int.Parse(jCointaner["intervalCancelOrder"].ToString());
            intervalOrder = int.Parse(jCointaner["intervalOrder"].ToString());
            intervalCapture = int.Parse(jCointaner["webserverIntervalCapture"].ToString());
            profit = double.Parse(jCointaner["profit"].ToString());
            fee = double.Parse(jCointaner["fee"].ToString());
            stoploss = double.Parse(jCointaner["stoploss"].ToString());
            stopgain = double.Parse(jCointaner["stopgain"].ToString());
            roeAutomatic = jCointaner["roe"].ToString() == "automatic";
            limiteOrder = int.Parse(jCointaner["limiteOrder"].ToString());

            bitMEXApi = new BitMEX.BitMEXApi(bitmexKey, bitmexSecret, bitmexDomain);

            System.Threading.Thread.Sleep(60000 * 10);

           
            




            //TESTS HERE

            //makeOrder("Buy");

            //FINAL

            if (configJson.webserver == "enable")
            {
                WebServer ws = new WebServer(WebServer.SendResponse, configJson.webserverConfig);
                ws.Run();
                System.Threading.Thread tCapture = new Thread(Database.captureDataJob);
                tCapture.Start();
                System.Threading.Thread.Sleep(1000);
                System.Diagnostics.Process.Start(configJson.webserverConfig);

            }



            log("Total open orders: " + bitMEXApi.GetOpenOrders(pair).Count);

            log("");
            log("Wallet: " + bitMEXApi.GetWallet());

            lstIndicatorsAll.Add(new IndicatorADX());
            lstIndicatorsAll.Add(new IndicatorMFI());
            lstIndicatorsAll.Add(new IndicatorBBANDS());
            lstIndicatorsAll.Add(new IndicatorCCI());
            lstIndicatorsAll.Add(new IndicatorCMO());
            lstIndicatorsAll.Add(new IndicatorDI());
            lstIndicatorsAll.Add(new IndicatorDM());
            lstIndicatorsAll.Add(new IndicatorMA());
            lstIndicatorsAll.Add(new IndicatorMACD());
            lstIndicatorsAll.Add(new IndicatorMOM());
            lstIndicatorsAll.Add(new IndicatorPPO());
            lstIndicatorsAll.Add(new IndicatorROC());
            lstIndicatorsAll.Add(new IndicatorRSI());
            lstIndicatorsAll.Add(new IndicatorSAR());
            lstIndicatorsAll.Add(new IndicatorSTOCH());
            lstIndicatorsAll.Add(new IndicatorSTOCHRSI());
            lstIndicatorsAll.Add(new IndicatorTRIX());
            lstIndicatorsAll.Add(new IndicatorULTOSC());
            lstIndicatorsAll.Add(new IndicatorWILLR());

            foreach (var item in configJson.indicatorsEntry)
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntry.Add(item2);
                    }
                }
            }

            foreach (var item in configJson.indicatorsEntryCross)
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntryCross.Add(item2);
                    }
                }
            }

            foreach (var item in configJson.indicatorsEntryDecision)
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntryDecision.Add(item2);
                    }
                }
            }


            


            bool automaticTendency = configJson.statusLong == "automatic";

            //LOOP 
            while (true)
            {
<<<<<<< HEAD
                try
                {

                    if (roeAutomatic)
                    {
                        positionContracts = getPosition(); // FIX CARLOS MORATO
                        roe = getRoe();
                    }
                    //double _priceContacts = Math.Abs(getPositionPrice());
                    //String _side = "Buy";
                    //if (positionContracts > 0)
                    //    _side = "Sell";
                    //double _actualPrice = Math.Abs(getPriceActual(_side));



                    //Stop Loss
                    if (roe < 0)
                    {
                        if ((roe * (-1)) >= stoploss)
                        {
                            //Stop loss
                            positionContracts = getPosition(); // FIX CARLOS MORATO
                            bitMEXApi.CancelAllOpenOrders(pair);
                            String side = "Buy";
                            if (positionContracts > 0)
                                side = "Sell";
                            bitMEXApi.MarketOrder(pair, side, positionContracts);
                            log("[STOP LOSS] - ROE " + roe + " positionContracts " + positionContracts + " side " + side + " ");
=======

                configJson.positionContracts = getPosition(); // FIX CARLOS MORATO
                roe = getRoe();


                //Stop Loss
                if (roe < 0)
                {
                    if ((roe * (-1)) >= configJson.stoploss)
                    {
                        //Stop loss
                        bitMEXApi.CancelAllOpenOrders(configJson.pair);
                        String side = "Buy";
                        if (configJson.positionContracts > 0)
                            side = "Sell";
                        bitMEXApi.MarketOrder(configJson.pair, side, configJson.positionContracts);
                    }
                }

                //Stop Gain
                if (roe > 0)
                {
                    if (roe >= configJson.stopgain)
                    {
                        //Stop loss
                        bitMEXApi.CancelAllOpenOrders(configJson.pair);
                        String side = "Buy";
                        if (configJson.positionContracts > 0)
                            side = "Sell";
                        bitMEXApi.MarketOrder(configJson.pair, side, configJson.positionContracts);
                    }
                }

                //SEARCH POSITION AND MAKE ORDER
                //By Carlos Morato
                if (configJson.roeAutomatic && (Math.Abs(getOpenOrderQty()) < Math.Abs(configJson.positionContracts)))
                {
                    log("Get Position " + configJson.positionContracts);

                    int qntContacts = (Math.Abs(configJson.positionContracts) - Math.Abs(getOpenOrderQty()));


                    if (configJson.positionContracts > 0)
                    {
                        string side = "Sell";
                        double priceContacts = Math.Abs(getPositionPrice());
                        double actualPrice = Math.Abs(getPriceActual(side));
                        double priceContactsProfit = Math.Abs(Math.Floor(priceContacts + (priceContacts * (configJson.profit + configJson.fee) / 100)));

                        if (actualPrice > priceContactsProfit)
                        {
                            double price = actualPrice + 1;
                            String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, qntContacts);
                            JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                            log(json);

>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                        }
                    }

                    //Stop Gain
                    if (roe > 0)
                    {
                        if (roe >= stopgain)
                        {
<<<<<<< HEAD
                            //Stop loss
                            positionContracts = getPosition(); // FIX CARLOS MORATO
                            bitMEXApi.CancelAllOpenOrders(pair);
                            String side = "Buy";
                            if (positionContracts > 0)
                                side = "Sell";
                            bitMEXApi.MarketOrder(pair, side, positionContracts);
                            log("[STOP GAIN] - ROE " + roe + " positionContracts " + positionContracts + " side " + side + " ");
                        }
                    }

                    //SEARCH POSITION AND MAKE ORDER
                    //By Carlos Morato
                    if (roeAutomatic && (Math.Abs(getOpenOrderQty()) < Math.Abs(positionContracts)))
                    {
                        log("Get Position " + positionContracts);
=======
                            double price = priceContactsProfit;
                            String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, qntContacts);
                            JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                            log(json);
                        }
                    }

                    if (configJson.positionContracts < 0)
                    {
                        string side = "Buy";
                        double priceContacts = Math.Abs(getPositionPrice());
                        double actualPrice = Math.Abs(getPriceActual(side));
                        double priceContactsProfit = Math.Abs(Math.Floor(priceContacts - (priceContacts * (configJson.profit + configJson.fee) / 100)));
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                        int qntContacts = (Math.Abs(positionContracts) - Math.Abs(getOpenOrderQty()));


                        if (positionContracts > 0)
                        {
<<<<<<< HEAD
                            string side = "Sell";
                            double priceContacts = Math.Abs(getPositionPrice());
                            double actualPrice = Math.Abs(getPriceActual(side));
                            double priceContactsProfit = Math.Abs(Math.Floor(priceContacts + (priceContacts * (profit + fee) / 100)));
=======
                            double price = actualPrice - 1;
                            String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, qntContacts);
                            JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                            log(json);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                            if (actualPrice > priceContactsProfit)
                            {
                                double price = actualPrice + 1;
                                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qntContacts);
                                JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                                log(json);

                            }
                            else
                            {
                                double price = priceContactsProfit;
                                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qntContacts);
                                JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                                log(json);
                            }
                        }

                        if (positionContracts < 0)
                        {
<<<<<<< HEAD
                            string side = "Buy";
                            double priceContacts = Math.Abs(getPositionPrice());
                            double actualPrice = Math.Abs(getPriceActual(side));
                            double priceContactsProfit = Math.Abs(Math.Floor(priceContacts - (priceContacts * (profit + fee) / 100)));
=======
                            double price = priceContactsProfit;
                            String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, qntContacts);
                            JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                            log(json);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                            if (actualPrice < priceContactsProfit)
                            {
                                double price = actualPrice - 1;
                                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qntContacts);
                                JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                                log(json);

                            }
                            else
                            {
                                double price = priceContactsProfit;
                                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qntContacts);
                                JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                                log(json);

                            }
                        }
                    }





<<<<<<< HEAD
                    //CANCEL ORDER WITHOUT POSITION
                    //By Carlos Morato
=======
                if (configJson.roeAutomatic && Math.Abs(configJson.positionContracts) != Math.Abs(getOpenOrderQty()))
                    bitMEXApi.CancelAllOpenOrders(configJson.pair);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                    if (roeAutomatic && Math.Abs(positionContracts) != Math.Abs(getOpenOrderQty()))
                        bitMEXApi.CancelAllOpenOrders(pair);

<<<<<<< HEAD
                    if (automaticTendency)
                        verifyTendency();
                    //GET CANDLES
                    if (getCandles())
=======
                    if (configJson.statusLong == "enable")
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                    {

                        if (statusLong == "enable")
                        {
                            log("");
                            log("==========================================================");
                            log(" ==================== Verify LONG OPERATION =============", ConsoleColor.Green);
                            log("==========================================================");
                            /////VERIFY OPERATION LONG
                            string operation = "buy";
                            //VERIFY INDICATORS ENTRY
                            foreach (var item in lstIndicatorsEntry)
                            {
                                Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                log("Indicator: " + item.getName());
                                log("Result1: " + item.getResult());
                                log("Result2: " + item.getResult2());
                                log("Operation: " + operationBuy.ToString());
                                log("");
                                if (operationBuy != Operation.buy)
                                {
                                    operation = "nothing";
                                    break;
                                }
                            }

                            //VERIFY INDICATORS CROSS
                            if (operation == "buy")
                            {
                                //Prepare to long                        
                                while (true)
                                {
                                    log("wait operation long...");
                                    getCandles();
                                    foreach (var item in lstIndicatorsEntryCross)
                                    {
                                        Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                        log("Indicator Cross: " + item.getName());
                                        log("Result1: " + item.getResult());
                                        log("Result2: " + item.getResult2());
                                        log("Operation: " + operationBuy.ToString());
                                        log("");

                                        if (item.getTypeIndicator() == TypeIndicator.Cross)
                                        {
                                            if (operationBuy == Operation.buy)
                                            {
                                                operation = "long";
                                                break;
                                            }
                                        }
                                        else if (operationBuy != Operation.buy)
                                        {
                                            operation = "long";
                                            break;
                                        }
                                    }
                                    if (lstIndicatorsEntryCross.Count == 0)
                                        operation = "long";
                                    if (operation != "buy")
                                        break;
<<<<<<< HEAD
                                    log("wait " + interval + "ms");
                                    Thread.Sleep(interval);
=======
                                    }
                                }
                                if (lstIndicatorsEntryCross.Count == 0)
                                    operation = "long";
                                if (operation != "buy")
                                    break;
                                log("wait " + configJson.interval + "ms");
                                Thread.Sleep(configJson.interval);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                                }
                            }

                            //VERIFY INDICATORS DECISION
                            if (operation == "long" && lstIndicatorsEntryDecision.Count > 0)
                            {
                                operation = "decision";
                                foreach (var item in lstIndicatorsEntryDecision)
                                {
                                    Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                    log("Indicator Decision: " + item.getName());
                                    log("Result1: " + item.getResult());
                                    log("Result2: " + item.getResult2());
                                    log("Operation: " + operationBuy.ToString());
                                    log("");


<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" && getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
=======
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" &&
                                    configJson.getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
                                {
                                    int decisionPoint = int.Parse(configJson.getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                    if (item.getResult() >= decisionPoint && item.getTendency() == Tendency.high)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                        if (item.getResult() >= decisionPoint && item.getTendency() == Tendency.high)
                                        {
                                            operation = "long";
                                            break;
                                        }
                                    }

<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")
=======
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")
                                {
                                    int decisionPoint = int.Parse(configJson.getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                    if (item.getResult() >= decisionPoint)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                        if (item.getResult() >= decisionPoint)
                                        {
                                            operation = "long";
                                            break;
                                        }
                                    }
<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
=======
                                }
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
                                {
                                    if (item.getTendency() == Tendency.high)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        if (item.getTendency() == Tendency.high)
                                        {
                                            operation = "long";
                                            break;
                                        }
                                    }

                                }
                            }


                            //EXECUTE OPERATION
                            if (operation == "long")
                                makeOrder("Buy");

                            ////////////FINAL VERIFY OPERATION LONG//////////////////
                        }


<<<<<<< HEAD
                        if (statusShort == "enable")
=======
                    if (configJson.statusShort == "enable")
                    {
                        //////////////////////////////////////////////////////////////
                        log("");
                        log("==========================================================");
                        log(" ==================== Verify SHORT OPERATION =============", ConsoleColor.Red);
                        log("==========================================================");
                        /////VERIFY OPERATION LONG
                        string operation = "sell";
                        //VERIFY INDICATORS ENTRY
                        foreach (var item in lstIndicatorsEntry)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                        {
                            //////////////////////////////////////////////////////////////
                            log("");
                            log("==========================================================");
                            log(" ==================== Verify SHORT OPERATION =============", ConsoleColor.Red);
                            log("==========================================================");
                            /////VERIFY OPERATION LONG
                            string operation = "sell";
                            //VERIFY INDICATORS ENTRY
                            foreach (var item in lstIndicatorsEntry)
                            {
                                Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                log("Indicator: " + item.getName());
                                log("Result1: " + item.getResult());
                                log("Result2: " + item.getResult2());
                                log("Operation: " + operationBuy.ToString());
                                log("");
                                if (operationBuy != Operation.sell)
                                {
                                    operation = "nothing";
                                    break;
                                }
                            }

                            //VERIFY INDICATORS CROSS
                            if (operation == "sell")
                            {
                                //Prepare to long                        
                                while (true)
                                {
                                    log("wait operation short...");
                                    getCandles();
                                    foreach (var item in lstIndicatorsEntryCross)
                                    {
                                        Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                        log("Indicator Cross: " + item.getName());
                                        log("Result1: " + item.getResult());
                                        log("Result2: " + item.getResult2());
                                        log("Operation: " + operationBuy.ToString());
                                        log("");

                                        if (item.getTypeIndicator() == TypeIndicator.Cross)
                                        {
                                            if (operationBuy == Operation.sell)
                                            {
                                                operation = "short";
                                                break;
                                            }
                                        }
                                        else if (operationBuy != Operation.sell)
                                        {
                                            operation = "short";
                                            break;
                                        }
                                    }
                                    if (lstIndicatorsEntryCross.Count == 0)
                                        operation = "short";
                                    if (operation != "sell")
                                        break;
<<<<<<< HEAD
                                    log("wait " + interval + "ms");
                                    Thread.Sleep(interval);
=======
                                    }
                                }
                                if (lstIndicatorsEntryCross.Count == 0)
                                    operation = "short";
                                if (operation != "sell")
                                    break;
                                log("wait " + configJson.interval + "ms");
                                Thread.Sleep(configJson.interval);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

                                }
                            }

                            //VERIFY INDICATORS DECISION
                            if (operation == "short" && lstIndicatorsEntryDecision.Count > 0)
                            {
                                operation = "decision";
                                foreach (var item in lstIndicatorsEntryDecision)
                                {
                                    Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                                    log("Indicator Decision: " + item.getName());
                                    log("Result1: " + item.getResult());
                                    log("Result2: " + item.getResult2());
                                    log("Operation: " + operationBuy.ToString());
                                    log("");


<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" && getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
=======
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" && 
                                    configJson.getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
                                {
                                    int decisionPoint = int.Parse(configJson.getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                    if (item.getResult() <= decisionPoint && item.getTendency() == Tendency.low)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                        if (item.getResult() <= decisionPoint && item.getTendency() == Tendency.low)
                                        {
                                            operation = "short";
                                            break;
                                        }
                                    }

<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")
=======
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")
                                {
                                    int decisionPoint = int.Parse(configJson.getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                    if (item.getResult() <= decisionPoint)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                                        if (item.getResult() <= decisionPoint)
                                        {
                                            operation = "short";
                                            break;
                                        }
                                    }
<<<<<<< HEAD
                                    if (getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
=======
                                }
                                if (configJson.getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")
                                {
                                    if (item.getTendency() == Tendency.low)
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                                    {
                                        if (item.getTendency() == Tendency.low)
                                        {
                                            operation = "short";
                                            break;
                                        }
                                    }

                                }
                            }


                            //EXECUTE OPERATION
                            if (operation == "short")
                                makeOrder("Sell");

                            ////////////FINAL VERIFY OPERATION LONG//////////////////
                        }

                    }
                    log("wait " + interval + "ms", ConsoleColor.Blue);
                    Thread.Sleep(interval);
                }
                catch (Exception ex)
                {
                    log("while true::" + ex.Message + ex.StackTrace);
                }
<<<<<<< HEAD
=======
                log("wait " + configJson.interval + "ms", ConsoleColor.Blue);
                Thread.Sleep(configJson.interval);

>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
            }

        }
        catch (Exception ex)
        {
            log("ERROR FATAL::" + ex.Message + ex.StackTrace);
        }
    }


    static bool existsOrderOpenById(string id)
    {
        List<BitMEX.Order> lst = bitMEXApi.GetOpenOrders(configJson.pair);
        foreach (var item in lst)
        {
            if (item.OrderId.ToUpper().Trim() == id.ToUpper().Trim())
                return true;
        }
        return false;
    }


    static void makeOrder(string side)
    {        
        bool execute = false;
        try
        {
            log("Make order " + side);
<<<<<<< HEAD
            
            if (side == "Sell" && statusShort == "enable" && Math.Abs(limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(pair).Count))
            {
                double price = Math.Abs(getPriceActual(side)) + stepValue;
                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qtdyContacts);
                JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                log(json);                
                
                for (int i = 0; i < intervalCancelOrder; i++)
                {
                    if (!existsOrderOpenById(jCointaner["orderID"].ToString()))
                    {
                        if (scalper)
                        {
                            price = price - stepValue;
                        }
                        else
                        {
                            price -= (price * profit) / 100;
                            price = Math.Abs(Math.Floor(price));
                        }
                        json = bitMEXApi.PostOrderPostOnly(pair, "Buy", price, qtdyContacts);
=======

            if (side == "Sell" && configJson.statusShort == "enable" && Math.Abs(configJson.limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(configJson.pair).Count))
            {
                double price = Math.Abs(getPriceActual(side) + 1);
                String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, configJson.qtdyContacts);
                JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                log(json);
                log("wait total...");
                for (int i = 0; i < configJson.intervalCancelOrder; i++)
                {
                    if (!existsOrderOpenById(jCointaner["orderID"].ToString()))
                    {
                        price -= (price * configJson.profit) / 100;
                        price = Math.Abs(Math.Floor(price));
                        json = bitMEXApi.PostOrderPostOnly(configJson.pair, "Buy", price, configJson.qtdyContacts);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                        log(json);
                        execute = true;
                        break;
                    }
                    log("wait order limit " + i + " of " + intervalCancelOrder + "...");
                    Thread.Sleep(800);
                }



                if (!execute)
                {
                    bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    while (existsOrderOpenById(jCointaner["orderID"].ToString()))
                        bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    log("Cancel order ID " + jCointaner["orderID"].ToString());
                }

            }
<<<<<<< HEAD
            if (side == "Buy" && statusLong == "enable" && Math.Abs(limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(pair).Count))            
            {
                double price = Math.Abs(getPriceActual(side)) - stepValue;
                String json = bitMEXApi.PostOrderPostOnly(pair, side, price, qtdyContacts);
=======
            if (side == "Buy" && configJson.statusLong == "enable" && Math.Abs(configJson.limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(configJson.pair).Count))
            {
                double price = Math.Abs(getPriceActual(side) - 1);
                String json = bitMEXApi.PostOrderPostOnly(configJson.pair, side, price, configJson.qtdyContacts);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                log(json);
                log("wait total...");
                for (int i = 0; i < configJson.intervalCancelOrder; i++)
                {
                    if (!existsOrderOpenById(jCointaner["orderID"].ToString()))
                    {
<<<<<<< HEAD
                        if (scalper)
                        {
                            price = price + stepValue;
                        }
                        else
                        {
                            price += (price * profit) / 100;
                            price = Math.Abs(Math.Floor(price));
                        }
                        json = bitMEXApi.PostOrderPostOnly(pair, "Sell", price, qtdyContacts);
=======
                        price += (price * configJson.profit) / 100;
                        price = Math.Abs(Math.Floor(price));
                        json = bitMEXApi.PostOrderPostOnly(configJson.pair, "Sell", price, configJson.qtdyContacts);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
                        log(json);
                        execute = true;
                        break;
                    }
                    log("wait order limit " + i + " of " + intervalCancelOrder + "...");
                    Thread.Sleep(800);
                }


                if (!execute)
                {
                    bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    while (existsOrderOpenById(jCointaner["orderID"].ToString()))
                        bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    log("Cancel order ID " + jCointaner["orderID"].ToString());
                }


            }
        }
        catch (Exception ex)
        {
            log("makeOrder()::" + ex.Message + ex.StackTrace);
        }

        if (execute)
        {
<<<<<<< HEAD
            log("wait " + intervalOrder + "ms", ConsoleColor.Blue);
            Thread.Sleep(intervalOrder);
=======
            log("wait " + configJson.intervalOrder * 2 + "ms", ConsoleColor.Blue);
            Thread.Sleep(configJson.intervalOrder * 2);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103

        }
    }

    static void verifyTendency()
    {
        try
        {
            String json = Http.get("https://api.binance.com/api/v1/ticker/24hr?symbol=BTCUSDT");
            JContainer j = (Newtonsoft.Json.Linq.JContainer)JsonConvert.DeserializeObject(json);

            tendencyMarket = TendencyMarket.NORMAL;
            decimal priceChangePercent = decimal.Parse(j["priceChangePercent"].ToString().Replace(".", ","));
            if (priceChangePercent < -1.0m)
                tendencyMarket = TendencyMarket.LOW;
            if (priceChangePercent > -1.0m && priceChangePercent < 1.5m)
                tendencyMarket = TendencyMarket.NORMAL;
            if (priceChangePercent > 1.5m)
                tendencyMarket = TendencyMarket.HIGH;
            if (priceChangePercent < -2)
                tendencyMarket = TendencyMarket.VERY_LOW;
            if (priceChangePercent > 3.5m)
                tendencyMarket = TendencyMarket.VERY_HIGH;


            if (tendencyMarket == TendencyMarket.VERY_HIGH || tendencyMarket == TendencyMarket.HIGH)
            {
                configJson.statusShort = "disable";
                configJson.statusLong = "enable";
            }
            else if (tendencyMarket == TendencyMarket.NORMAL)
            {
                configJson.statusShort = "enable";
                configJson.statusLong = "enable";
            }
            else if (tendencyMarket == TendencyMarket.LOW || tendencyMarket == TendencyMarket.VERY_LOW)
            {
                configJson.statusShort = "enable";
                configJson.statusLong = "disable";
            }

            //if (tendencyMarket == TendencyMarket.VERY_HIGH || tendencyMarket == TendencyMarket.VERY_LOW)
              //  timeGraph = "1m";
            //else
              //  timeGraph = "5m";
        }
        catch (Exception ex)
        {

        }
    }


    static double getPriceActual(string type)
    {
        List<BitMEX.OrderBook> listBook = bitMEXApi.GetOrderBook(configJson.pair, 1);
        foreach (var item in listBook)
        {
            if (item.Side.ToUpper() == type.ToUpper())
                return item.Price;
        }

        return 0;
    }

    static bool getCandles()
    {
        try
        {
            arrayPriceClose = new double[100];
            arrayPriceHigh = new double[100];
            arrayPriceLow = new double[100];
            arrayPriceVolume = new double[100];
            arrayPriceOpen = new double[100];
            List<BitMEX.Candle> lstCandle = bitMEXApi.GetCandleHistory(configJson.pair, 100, configJson.timeGraph);
            int i = 0;
            foreach (var candle in lstCandle)
            {
                arrayPriceClose[i] = (double)candle.Close;
                arrayPriceHigh[i] = (double)candle.High;
                arrayPriceLow[i] = (double)candle.Low;
                arrayPriceVolume[i] = (double)candle.Volume;
                arrayPriceOpen[i] = (double)candle.Open;
                i++;
            }
            Array.Reverse(arrayPriceClose);
            Array.Reverse(arrayPriceHigh);
            Array.Reverse(arrayPriceLow);
            Array.Reverse(arrayPriceVolume);
            Array.Reverse(arrayPriceOpen);




            Console.Title = DateTime.Now.ToString() + " - " + configJson.pair + " - $ " + arrayPriceClose[99].ToString() + " v" + version + " - " + configJson.bitmexDomain + " | Price buy " + getPriceActual("Buy") + " | Price Sell " + getPriceActual("Sell") + " | " + tendencyMarket + "| Roe " + roe;
            return true;
        }
        catch (Exception ex)
        {
            log("GETCANDLES::" + ex.Message + ex.StackTrace);
            //log("wait " + intervalOrder + "ms");
            //Thread.Sleep(intervalOrder);
            return false;
        }

    }

    //By Lucas Sousa modify MatheusGrijo
    static int getPosition()
    {
        try
        {
<<<<<<< HEAD
            log("getPosition...");
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(pair);
=======
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(configJson.pair);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
            int _qtdContacts = 0;
            foreach (var Position in OpenPositions)
                _qtdContacts += (int)Position.CurrentQty;
            log("getPosition: " + _qtdContacts);
            return _qtdContacts;
        }
        catch (Exception ex)
        {
            log("getPosition::" + ex.Message + ex.StackTrace);
            throw new Exception("Error getPosition");
        }
    }

    static double getRoe()
    {
        try
        {
<<<<<<< HEAD
            log("getRoe...");
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(pair);
=======
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(configJson.pair);
>>>>>>> a75012c1b05a63125334c9c3117159cc5664e103
            double _roe = 0;
            foreach (var Position in OpenPositions)
                _roe += Position.percentual();
            log("getRoe: " + _roe);
            return _roe;
        }
        catch (Exception ex)
        {
            log("getRoe::" + ex.Message + ex.StackTrace);
            throw new Exception("Error getRoe");
        }
    }

    //GetOpenOrderQty
    //by Carlos Morato
    static int getOpenOrderQty()
    {
        try
        {
            List<BitMEX.Order> OpenOrderQty = bitMEXApi.GetOpenOrders(configJson.pair);
            int _contactsQty = 0;
            foreach (var Order in OpenOrderQty)
                if (Order.Side == "Sell")
                    _contactsQty += (int)Order.OrderQty * (-1);
                else
                    _contactsQty += (int)Order.OrderQty;
            return _contactsQty;
        }
        catch (Exception ex)
        {
            log("getOpenOrderQty::" + ex.Message + ex.StackTrace);
            return 0;
        }
    }


    //GetPositionPrice
    //by Carlos Morato
    static double getPositionPrice()
    {
        try
        {
            List<BitMEX.Position> OpenPositionsPrice = bitMEXApi.GetOpenPositions(configJson.pair);
            double _priceContacts = 0;
            foreach (var Position in OpenPositionsPrice)
                _priceContacts = (double)Position.AvgEntryPrice;
            return _priceContacts;
        }
        catch (Exception ex)
        {
            log("getPositionPrice::" + ex.Message + ex.StackTrace);
            return 0;
        }
    }

    static void log(string value, ConsoleColor color = ConsoleColor.White)
    {
        try
        {

            value = "[" + DateTime.Now.ToString() + "] - " + value;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;

            System.IO.StreamWriter w = new StreamWriter(location + DateTime.Now.ToString("yyyyMMdd") + "_log.txt", true);
            w.WriteLine(value);
            w.Close();
            w.Dispose();

        }
        catch { }
    }



}





