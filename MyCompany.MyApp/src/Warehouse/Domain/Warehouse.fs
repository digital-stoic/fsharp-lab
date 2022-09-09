namespace MyCompany.MyApp.Warehouse.Domain

type CodeStatus = | NotImplemented

type CaskId = string
type CaskSize = int

// Info from https://www.whiskyinvestdirect.com/about-whisky/scotch-whisky-casks-and-barrels
type CaskType =
    | QuarterCask
    | Barrel
    | Hogshead
    | Barrique
    | Puncheon
    | Butt
    | PortPipe
    | MadeiraDrum

// TODO: CaskType conversions
module CaskType =
    let sizeRange caskType : (CaskSize * CaskSize) =
        match caskType with
        | QuarterCask -> (45, 50)
        | Barrel -> (190, 200)
        | Hogshead -> (225, 250)
        | Barrique -> (250, 300)
        | Puncheon -> (450, 500)
        | Butt -> (475, 500)
        | PortPipe -> (550, 650)
        | MadeiraDrum -> (600, 650)

    let size caskType : CaskSize =
        let (min, max) = sizeRange caskType
        (min + max) / 2


    // LPA: Litre of Pure Alchohol
    let lpa = NotImplemented

// TODO: Cask type
type Cask =
    { Id: CaskId
      Type: CaskType
      Size: CaskSize }

module Cask =
    let create caskId caskType =
        let size = CaskType.size caskType

        { Id = caskId
          Type = caskType
          Size = size }
