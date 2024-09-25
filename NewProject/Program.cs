using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.IO;

namespace NewProject
{
    internal class Program
    {


        class Item
        {
            //아이템에게 필요한 변수
            public string itemName;
            public int itemDef;
            public int itemPower;
            public string iteminfo;
            public int itemGold;
            public bool isEquipped; //장착 했는지 안했는지
            public bool isBought; //샀는지 안샀는지

            //아이템 변수 초기화 및 아이템 생성자
            public Item(string name, int def,int power,string info) 
            {
                itemName = name;
                itemDef = def;
                itemPower = power;
                iteminfo = info;
                isEquipped = false;

            }

            public Item(string name, int def, int power, string info,int gold)
            {
                itemName = name;
                itemDef = def;
                itemPower = power;
                iteminfo = info;
                itemGold = gold;
                isBought = false;
            }

            //복사 생성자 아이템을 상점에서 사고 인
            //public Item(Item item)
            //{
            //    this.itemName = item.itemName;
            //    this.itemDef = item.itemDef;
            //    this.itemPower = item.itemPower;
            //    this.iteminfo = item.iteminfo;
            //    this.itemGold = item.itemGold;
            //    this.isEquipped = item.isEquipped;
            //    this.isBought = item.isBought;
            //}
        }

        class Player
        {
            //던전 관련 플레이어 레벨없
            public int dungeonClearCount; // 던전 클리어 횟수
            private int requiredClearsToLevelUp = 1; // 레벨업을 위한 클리어 횟수

            //플레이어 속성
            public string playerName;
            public int level;
            public int power;
            public int defense;
            public int health;
            public int money;
            public List<Item> inventory;//아이템 받기
            public List<Item> shopItem;//상점아이템 받기

            //플레이어 생성
            public Player(string name)
            {
                playerName = name;
                level = 1;
                power = 6;
                defense = 3;
                health = 100;
                money = 1500;
                inventory = new List<Item>(); // 인벤토리 초기화
                shopItem = new List<Item>(); // 상점아이템 초기화
            }

            public void LevelUp()
            {
                if(dungeonClearCount>=requiredClearsToLevelUp)
                {
                    level++;
                    dungeonClearCount = 0;
                    requiredClearsToLevelUp = level;

                    //레벨업 주는 보상
                    power += 1;
                    defense += 1;

                    Console.WriteLine("\n레벨업을 축하합니다!!!!\n");
                    Console.WriteLine("공격력 +1");
                    Console.WriteLine("방어력 +1");
                    Console.WriteLine($"플레이어 능력치 : 공격력 : {power} | 방어력 : {defense} 입니다");
                    GameStart.Instance.ActionSelect();
                }
            }

             



            public int PlusPower()
            {
                int totalPower=0; //지역변수로 만들 때 초기화 해주기 

                foreach(Item item in inventory)
                {
                    if(item.isEquipped)
                    {
                        totalPower += item.itemPower;
                    }
                   
                }

                return totalPower; //아이템 공격력만 받은 상태
            }

            public int PlusDefense()
            {
                int totalDefense = 0;

                foreach (var item in inventory)
                {
                    if(item.isEquipped)
                    {
                        totalDefense += item.itemDef;
                    }
                }

                return totalDefense; 
            }

            //플레이어 상태창 보기
            public void ShowState()
            {

                Console.WriteLine("==========================================");
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine("");
                Console.WriteLine($"Lv. {level}");
                Console.WriteLine($"이름 : {playerName}");
                Console.WriteLine($"공격력 : {PlusPower()+power} {(PlusPower()>0? $"({PlusPower()})" : "")}");
                Console.WriteLine($"방어력 : {PlusDefense()+defense} {(PlusDefense() > 0 ? $"({PlusDefense()})" : "")}");
                Console.WriteLine($"쳬력 : {health}");
                Console.WriteLine($"Gold : {money} G");
                Console.WriteLine("");

            }

            public void InventoryAdd(Item item) 
            {
                inventory.Add(item); //아이템 추가 List기능중에 add사용
            }

            public void ShopItemAdd(Item item)
            {
                shopItem.Add(item);//아이템 추가 List기능중에 add사용
            }

            public void ShowInventory()
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                if(inventory.Count ==0)
                {
                    Console.WriteLine("");
                }
                else 
                {
                    foreach(Item item in inventory)
                    {
                        string equipMark = item.isEquipped ? "[E]" : ""; //장착 했다는 표시 
                        string itemType = item.itemPower>0? $"공격력 +{item.itemPower}" : $"방어력 +{item.itemDef}";
                        Console.WriteLine($"{equipMark} {item.itemName}  | {itemType} | {item.iteminfo}");
                    }
                }

                Console.WriteLine("");
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                
                
           



            }

            
            public void EquipManagement() //장착 관리 하는 함수
            {
                Console.WriteLine("");
                Console.WriteLine("인벤토리-장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");

                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    string equippedMarker = item.isEquipped ? "[E]" : "";
                    Console.WriteLine($"{i + 1}. {equippedMarker} {item.itemName}");
                }
                Console.WriteLine("");
                Console.WriteLine("장착할 아이템 번호를 선택하세요(0번 입력 시 인벤토리):");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index > 0 && index <= inventory.Count)
                {
                    var selectedItem = inventory[index - 1];

                    // 장착 관리
                    if (selectedItem.isEquipped)
                    {
                        Console.WriteLine($"{selectedItem.itemName}을(를) 장착 해제했습니다.");
                        selectedItem.isEquipped = false;
                        EquipManagement();
                    }
                    else
                    {
                        Console.WriteLine($"{selectedItem.itemName}을(를) 장착했습니다.");
                        selectedItem.isEquipped = true;
                        EquipManagement();

                    }
                
                }
                else if(index ==0)
                {
                    ShowInventory();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    EquipManagement();
                }
            }

          

            public void ShowShop()
            {
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{money} G");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                foreach (var item in shopItem)
                {
                    //string buyMark = item.isBought ? "구매완료" : "";
                    string itemType = item.itemPower > 0 ? $"공격력 +{item.itemPower}" : $"방어력 +{item.itemDef}";
                    Console.WriteLine($" {item.itemName}  | {itemType} | {item.iteminfo} | {(item.isBought ? "구매완료" : $"{item.itemGold} G")} ");

                }

            }

            public void Buying()
            {
                Console.WriteLine("상점- 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{money} G");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");


                for (int i = 0; i < shopItem.Count; i++)
                {
                    Item item = shopItem[i];
                    string itemType = item.itemPower > 0 ? $"공격력 +{item.itemPower}" : $"방어력 +{item.itemDef}";
                    string buyMark = item.isBought ? "구매완료" : $"{item.itemGold} G";
                    Console.WriteLine($"{i + 1}. {item.itemName} | {itemType} | {item.iteminfo} | {buyMark}");
                }

                Console.Write("\n구매할 아이템 번호를 입력하세요(0번 입력 시 상점)  : ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int index) && index > 0 && index <= shopItem.Count)
                {
                    Item selectedItem = shopItem[index - 1];

                    // 이미 구매한 아이템 확인
                    if (selectedItem.isBought)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Buying();
                    }
                    else if (money >= selectedItem.itemGold) // 골드가 충분한지 확인
                    {
                        // 골드 차감 및 아이템 구매 처리
                        money -= selectedItem.itemGold;
                        selectedItem.isBought = true;

                        // 인벤토리에 추가
                        InventoryAdd(selectedItem);

                        Console.WriteLine($"'{selectedItem.itemName}' 아이템을 {selectedItem.itemGold} G에 구매 완료했습니다.");
                        Buying();
                    }
                    else
                    {
                        // 골드 부족
                        Console.WriteLine("Gold가 부족합니다.");
                        Buying();
                    }
                }
                else
                {

                    GameStart.Instance.ActionSelect();
                }
            }

            public void Sell()
            {
                Console.WriteLine("상점- 아이템 판매");
                Console.WriteLine("필요없는 아이템을 팔 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{money} G");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < inventory.Count; i++)
                {
                    Item item = inventory[i];
                    string equipMark = item.isEquipped ? "[E]" : ""; // 장착 여부 표시
                    string itemType = item.itemPower > 0 ? $"공격력 +{item.itemPower}" : $"방어력 +{item.itemDef}";
                    string buyMark = item.isBought ? "구매완료" : $"{item.itemGold} G";
                    if(buyMark == "구매완료")
                    {
                        
                        Console.WriteLine($"- {i + 1}. {equipMark} {item.itemName} | {itemType} | {item.iteminfo} | {item.itemGold} G");
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                }

                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string number = Console.ReadLine();

                if(int.TryParse(number, out int index) && index > 0 && index <= inventory.Count)
                {
                    Item selectedItem = inventory[index - 1];

                    // 판매 가격 계산 (85%로 판매)
                    int sellPrice = (int)(selectedItem.itemGold * 0.85);

                    // 장착되어 있다면 해제
                    if (selectedItem.isEquipped)
                    {
                        Console.WriteLine($"{selectedItem.itemName}은(는) 장착 중인 상태였습니다. 장착 해제 후 판매됩니다.");
                        selectedItem.isEquipped = false;
                    }

                    // 골드 추가 및 인벤토리에서 아이템 제거
                    money += sellPrice;
                    inventory.Remove(selectedItem);

                    Console.WriteLine($"{selectedItem.itemName}을(를) {sellPrice} G에 판매했습니다.");
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

            }



            public void RestTime()
            {
                Console.WriteLine("\n휴식하기");
                Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다.(보유 골드 : {money} G)");

                Console.WriteLine("\n1. 휴식하기");
                Console.WriteLine("0. 나가기");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string number = Console.ReadLine();

                if(number =="1")
                {
                    if(money>= 500)
                    {
                        Console.WriteLine("휴식을 완료했습니다.");
                        money -= 500;
                        health = 100;
                        RestTime();
                    }
                    else
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        RestTime();
                    }
                }
                else
                {
                    GameStart.Instance.ActionSelect();
                }
            }
        }


        class GameStart 
        {
            private Player player; //player클래스 정보 가져오기
            public static GameStart Instance;

            public void Start()
            {
                Instance = this;
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            }

            //이름 정하기
            public string NameSelect()
            {
                Console.Write("이름을 설정해주세요 :");
                string name = Console.ReadLine();
                Console.WriteLine("\n");
                Console.WriteLine("입력하신 이름은{0}입니다.", name);

                player = new Player(name);

                // 테스트용 아이템 추가
                //player.InventoryAdd(new Item("무쇠갑옷", 0, 5, "무쇠로 만들어져 튼튼한 갑옷입니다." ));
                //player.InventoryAdd(new Item("스파르타의 창", 7, 0, "스파르타 전사들이 사용한 전설의 창입니다."));
                //player.InventoryAdd(new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다."));

                //상점 아이템 추가
                player.ShopItemAdd(new Item("수련자 갑옷", 5, 0, "수련에 도움을 주는 갑옷입니다.",1000));
                player.ShopItemAdd(new Item("무쇠갑옷  ", 9, 0, "무쇠로 만들어져 튼튼한 갑옷입니다.",2000));
                player.ShopItemAdd(new Item("스파르타의 갑옷  ", 15, 0, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
                player.ShopItemAdd(new Item("낡은 검  ", 0, 2, "쉽게 볼 수 있는 낡은 검 입니다. ", 600));
                player.ShopItemAdd(new Item("적당한 도끼  ", 0, 5, " 어디선가 사용됐던거 같은 도끼입니다. ", 1500));
                player.ShopItemAdd(new Item("강력한 창  ", 0, 7, " 스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500));
                player.ShopItemAdd(new Item("무식한 검  ", 0, 10, " 무식한 검사가 사용했던 검입니다. ", 3000));

                ActionSelect();

                return name;

            }

            //플레이어 행동 
            public void ActionSelect()
            {
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 선택해주세요 ");
                Console.WriteLine("1. 상태 정보 ");
                Console.WriteLine("2. 상점 가기 ");
                Console.WriteLine("3. 인벤토리 ");
                Console.WriteLine("4. 휴식하기 ");
                Console.WriteLine("5. 던전입장 ");
                Console.WriteLine("");
                string input = Console.ReadLine();

                while(true)
                {
                    switch (input)
                    {
                        case "1":
                            GoToState();
                            break;
                        case "2":
                            GetShop();
                            break;
                        case "3":
                            Inventory();
                            break;
                        case "4":
                            Rest();
                            break;
                        case "5":
                            EnterDungeon();
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            ActionSelect();
                            break;
                    }
                    break; // 유효한 입력이 있을 경우 루프 탈출
                }
            }

            public void GoToState()
            {
                player.ShowState();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하는 행동을 입력해주세요");
                string number = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("==========================================");

                if (number != "0")
                {
                    Console.WriteLine("다시 입력하세요");
                }
                else
                {
                    ActionSelect();
                }
            }

            public void GetShop()
            {
                player.ShowShop();
                Console.WriteLine("");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.Buying();
                        break;
                    case "2":
                        player.Sell();
                        break;
                    case "0":
                        ActionSelect();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                Console.WriteLine("\n0번을 눌러 다시 행동을 선택하세요.");
                input = Console.ReadLine();
                while (input != "0")
                {
                    Console.WriteLine("잘못된 입력입니다. 0번을 눌러 다시 행동을 선택하세요.");
                    input = Console.ReadLine();
                }
                ActionSelect();
            }

            public void Inventory()
            {
                player.ShowInventory();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.EquipManagement();
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                Console.WriteLine("\n0번을 눌러 다시 행동을 선택하세요.");
                input = Console.ReadLine();
                while (input != "0")
                {
                    Console.WriteLine("잘못된 입력입니다. 0번을 눌러 다시 행동을 선택하세요.");
                    input = Console.ReadLine();
                }
                ActionSelect();
            }

            public void Rest()
            {
                player.RestTime();
            }

            // 던전 선택
            public void EnterDungeon()
            {
                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전   | 방어력 17 이상 권장");
                Console.WriteLine("0. 나가기");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if(player.health >0)
                {
                    switch (input)
                    {
                        case "1":
                            DungeonClear(5, 1000, "쉬운 던전");
                            break;
                        case "2":
                            DungeonClear(11, 1700, "일반 던전");
                            break;
                        case "3":
                            DungeonClear(17, 2500, "어려운 던전");
                            break;
                        case "0":
                            ActionSelect();
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            EnterDungeon();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("휴식을 취하고 오세요\n");
                    ActionSelect();
                }
               
            }

            // 던전 클리어 처리
            public void DungeonClear(int requiredDefense, int baseGold, string dungeonName) //쉬운 ,보통,어려움 다 똑같은 매개변수를 가지고 있기 대문에 
            {
                int playerDefense = player.defense + player.PlusDefense();
                int playerPower = player.power + player.PlusPower();
                Random rand = new Random();

                // 방어력이 부족할 경우
                if (playerDefense < requiredDefense)
                {
                    if (rand.Next(0, 100) < 40) // 40% 실패 확률
                    {
                        Console.WriteLine($"{dungeonName} 도전에 실패했습니다.");
                        Math.Max(player.health /= 2,0);
                        Console.WriteLine($"체력이 절반으로 감소하였습니다. 현재 체력: {player.health}");
                        EnterDungeon();
                    }
                }

                // 체력 감소 계산
                int healthLoss = rand.Next(20, 35) + (requiredDefense - playerDefense);
                healthLoss = Math.Max(1, healthLoss); // 최소 체력 감소 1
                player.health -= healthLoss;

                // 추가 보상 계산
                int additionalGold = (int)(baseGold * (rand.Next(playerPower, playerPower * 2) / 100.0));
                player.money += baseGold + additionalGold;

                Console.WriteLine($"축하합니다!! {dungeonName}을(를) 클리어 하였습니다.");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {player.health + healthLoss} -> {player.health}");
                Console.WriteLine($"Gold {player.money - (baseGold + additionalGold)} G -> {player.money} G");

                //level 관련
                player.dungeonClearCount++;
                player.LevelUp();

                Console.WriteLine("\n0. 나가기");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    ActionSelect();
                }
            }
        }

       


        static void Main(string[] args)
        {
            GameStart gameStart = new GameStart();
            gameStart.Start();
            gameStart.NameSelect();
        }
    }
}
