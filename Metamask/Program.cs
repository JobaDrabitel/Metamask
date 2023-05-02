using Newtonsoft.Json;
using System;
using System.Net.Http;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.Merkle.Patricia;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;
using Nethereum.Model;
using Nethereum.Util;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Nethereum.RPC.Eth;
using Nethereum.Signer;

namespace Metamask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Создаем основной аккаунт (отправитель)
            //var gasPriceGwei = new Nethereum.Hex.HexTypes.HexBigInteger(10); // цена газа в Gwei
            //var gasLimit = new Nethereum.Hex.HexTypes.HexBigInteger(21000); // лимит газа
            //                                                                // Определяем параметры транзакции
            //var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(senderAccount.Address, BlockParameter.CreatePending());

            //var totalTransactionCost = Convert.ToInt64(gasLimit.ToString()) * Convert.ToInt64(value.ToString());

            //// Проверяем, что отправитель имеет достаточно средств для отправки транзакции
            //var balance = await web3.Eth.GetBalance.SendRequestAsync(senderAccount.Address, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

            //var balanceInWei = Web3.Convert.ToWei(balance.Value);
            //if (balanceInWei < totalTransactionCost)
            //{
            //    Console.WriteLine("Insufficient funds to send transaction.");
            //    return;
            //}
            // Создание экземпляра объекта транзакции
            var senderPrivateKey = "0x34a84615ee517a2efdbadfb9cf5e566700da9eace1677424cf4e28ef9b926b78"; // Замените на свой приватный ключ
           
            // Создаем аккаунт получателя
            var receiverAccountAddres = "0xd328769f759C4364be07e8344cbE028C9389915B"; // Замените на приватный ключ, созданный ранее
            // Устанавливаем параметры подключения к сети Ethereum
            //var rpcClient = new RpcClient(new Uri("1"));
            //var ecKey = new Nethereum.Signer.EthECKey(senderPrivateKey);
            var senderAccount = new Nethereum.Web3.Accounts.Account(senderPrivateKey);
            var web3 = new Web3(senderAccount);
            var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(senderAccount.Address, BlockParameter.CreatePending());

            var transaction = new TransactionInput
            {
                ChainId = new HexBigInteger(1),
                From = senderAccount.Address,
                To = receiverAccountAddres,
                Gas = new HexBigInteger(25000),
                GasPrice = new HexBigInteger(158912817),
                Value = new HexBigInteger(Web3.Convert.ToWei(1, UnitConversion.EthUnit.Ether)/1835),
                Nonce = new HexBigInteger(nonce),
                MaxFeePerGas = new HexBigInteger(158912817)
            };

            // Получение подписанной транзакции

            // Отправка транзакции
            var balance = await web3.Eth.GetBalance.SendRequestAsync(senderAccount.Address);
            Console.WriteLine($"Balance in Wei: {balance.Value}");
            //var t = new AccountOfflineTransactionSigner();
            //var a = await senderAccount.TransactionManager.SignTransactionAsync(transaction);
            //var sign = t.SignTransaction(senderAccount, transaction);
            var b = await web3.Eth.TransactionManager.SignTransactionAsync(transaction);
            var signedTransaction = senderAccount.TransactionManager.SignTransactionAsync(transaction).ToString();
            var transactionHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(b);


            //var trans = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync("0x87b13ee7e6a1fe4260120a30ad9b0954076da66432c77e425ecef1ad570b0ffc");/*web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash.ToString());*/
            //await Console.Out.WriteLineAsync(trans.Status.ToString());
            // Создаем транзакцию
            var transactiona = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(receiverAccountAddres, 35m, 2, new BigInteger(158912817));
            await Console.Out.WriteLineAsync(transactiona.Status.ToString());

        }
    


        static async Task CreateAccount(string privateKey)
        {

            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            string accountAddress = account.Address;
            await Console.Out.WriteLineAsync("Account address: " + accountAddress);
            await Console.Out.WriteLineAsync("Account private key: " + privateKey);
        }
    }
}
