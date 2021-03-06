module TransferEther

open System
open System.Numerics
open Nethereum.Web3
open Nethereum.Web3.Accounts
open Nethereum.Web3.Accounts.Managed
open Nethereum.Hex.HexTypes
open Nethereum.HdWallet

let printBalance =
    async {
        let web3 =
            Web3 "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c"

        let! balance =
            web3.Eth.GetBalance.SendRequestAsync "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae"
            |> Async.AwaitTask

        let etherAmount = Web3.Convert.FromWei balance.Value
        printfn $"Balance in Wei: {balance.Value}"
        printfn $"Balance in Ether: {etherAmount}"
    }

let sendTransaction =
    async {
        let privateKey =
            "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7"

        let account = Account privateKey
        let web3 = Web3 account

        let toAddress =
            "0x13f022d72158410433cbd66f5dd8bf6d2d129924"

        let! transaction =
            web3
                .Eth
                .GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toAddress, 1.11m, 2m, BigInteger 25000)
            |> Async.AwaitTask

        printfn "sendTransaction..."
    }

let sendWallet =
    async {
        let words =
            "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal"

        let password = "password"
        let wallet = Wallet(words, password)
        let account = wallet.GetAccount 0

        let toAddress =
            "0x13f022d72158410433cbd66f5dd8bf6d2d129924"

        let web3 = Web3 account

        let! transaction =
            web3
                .Eth
                .GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toAddress, 1.11m, 2m)
            |> Async.AwaitTask

        printfn "sendWallet..."
    }

let getBalance =
    let account =
        "0x12890d2cce102216644c59daE5baed380d84830c"

    async {
        let web3 = Web3()

        let! balance =
            web3.Eth.GetBalance.SendRequestAsync account
            |> Async.AwaitTask

        let balanceInWei = balance.Value
        let balanceInEther = Web3.Convert.FromWei balanceInWei

        printfn $"Balance of account {account} = {balanceInWei} wei"
        printfn $"Balance of account {account} = {balanceInEther} ETH"
    }
