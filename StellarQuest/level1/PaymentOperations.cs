using StellarDotnetSdk;
using StellarDotnetSdk.Accounts;
using StellarDotnetSdk.Assets;
using StellarDotnetSdk.Operations;
using StellarDotnetSdk.Transactions;

namespace StellarQuest.level1
{
    public static class PaymentOperations
    {
        private const string TestServerUri = "https://horizon-testnet.stellar.org";
        private const uint BaseFee = 1000;

        public static async void CreateAccount()
        {
            try
            {
                //const questKeypair = Keypair.fromSecret('SECRET_KEY_HERE')
                //const newKeypair = Keypair.random()

                var questKeypair = KeyPair.FromSecretSeed("SBBDF7MXPUN3EULKH6M74ND3R76BKIYGFOULL2QXCBABPJJHOFQUPYZZ");
                var newKeypair = KeyPair.Random();



                //const server = new Server('https://horizon-testnet.stellar.org')
                //const questAccount = await server.loadAccount(questKeypair.publicKey())

                var server = new Server(TestServerUri);
                var questAccountResponse = await server.Accounts.Account(questKeypair.AccountId);
                var questinAccount = await server.Accounts.Account(questAccountResponse.AccountId);



                //let transaction = new TransactionBuilder(
                //  questAccount, {
                //    fee: BASE_FEE,
                //    networkPassphrase: Networks.TESTNET
                //  })

                var transactionBuilder = new TransactionBuilder(questinAccount);
                transactionBuilder.SetFee(BaseFee);
                // network can be set right here:
                // Network.UseTestNetwork();
                // or when signing the transaction:
                // transaction.Sign(questKeypair, new Network(Network.TestnetPassphrase));



                //let transaction = new TransactionBuilder(...)
                //  .addOperation(Operation.createAccount({
                //            destination: newKeypair.publicKey(),
                //    startingBalance: "1000" // You can make this any amount you want (as long as you have the funds!)
                //  })

                var operation = new CreateAccountOperation(newKeypair, "1000");
                transactionBuilder.AddOperation(operation);



                //let transaction = new TransactionBuilder(...)
                //  .setTimeout(30)
                //  .build()
                //transaction.sign(questKeypair)

                transactionBuilder.AddTimeBounds(new TimeBounds(TimeSpan.FromMinutes(30)));
                var transaction = transactionBuilder.Build();
                transaction.Sign(questKeypair, new Network(Network.TestnetPassphrase));



                //console.log(transaction.toXdr())

                var xdr = transaction.ToXdr();



                //try
                //{
                //  let res = await server.submitTransaction(transaction)
                //  console.log(`Transaction Successful! Hash: ${ res.hash}`)
                //}
                //catch (error)
                //{
                //  console.log(`${ error}. More details:\n${ JSON.stringify(error.response.data.extras, null, 2)}`)
                //}

                try
                {
                    var response = await server.SubmitTransaction(transaction);
                    Console.WriteLine($"Transaction Successful! Hash: ${response!.Hash}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async void Payment()
        {
            try
            {
                //const questKeypair = Keypair.fromSecret('SECRET_KEY_HERE')
                //const destinationKeypair = Keypair.random()
                //await friendbot([questKeypair.publicKey(), destinationKeypair.publicKey()])

                var questKeypair = KeyPair.FromSecretSeed("SABKSIFASHE7PC4VQOXZMZSEDPVUM3SLPPC7H47B4I3YVJIYG573SFYJ");
                var destinationKeypair = KeyPair.Random();
                //await friendbot([questKeypair.publicKey(), destinationKeypair.publicKey()]) ?? Szabi



                //const server = new Server('https://horizon-testnet.stellar.org')
                //const questAccount = await server.loadAccount(questKeypair.publicKey())

                var server = new Server(TestServerUri);
                var accountResponse = await server.Accounts.Account(questKeypair.AccountId);
                var questAccount = await server.Accounts.Account(accountResponse.AccountId);



                //const transaction = new TransactionBuilder(
                //  questAccount, {
                //    fee: BASE_FEE,
                //    networkPassphrase: Networks.TESTNET
                //  })
                // .addOperation(Operation.payment({
                //    destination: destinationKeypair.publicKey(),
                //    asset: Asset.native(),
                //    amount: '100'
                // }))
                // .setTimeout(30)
                // .build()

                var transaction = new TransactionBuilder(accountResponse)
                    .SetFee(BaseFee)
                    .AddOperation(new PaymentOperation(
                        destination: KeyPair.FromAccountId(destinationKeypair.AccountId),
                        asset: new AssetTypeNative(),
                        amount: "100"))
                    .AddTimeBounds(new TimeBounds(TimeSpan.FromMinutes(30)))
                    .Build();



                //transaction.sign(questKeypair)

                transaction.Sign(questKeypair, new Network(Network.TestnetPassphrase));



                //try
                //{
                //  let res = await server.submitTransaction(transaction)
                //  console.log(`Transaction Successful! Hash: ${ res.hash}`)
                //}
                //catch (error)
                //{           {
                //  console.log(`${ error}. More details:\n${ JSON.stringify(error.response.data.extras, null, 2)}`)
                //}

                try
                {
                    var response = await server.SubmitTransaction(transaction);
                    Console.WriteLine($"Transaction Successful! Hash: ${response!.Hash}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
