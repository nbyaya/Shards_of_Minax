using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Lancelot")]
    public class SirLancealot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirLancealot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Lancelot";
            Body = 0x190; // Human male body

            // Stats
            Str = 158;
            Dex = 66;
            Int = 26;
            Hits = 113;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1400 });
            AddItem(new ChainChest() { Hue = 1400 });
            AddItem(new PlateHelm() { Hue = 1400 });
            AddItem(new PlateGloves() { Hue = 1400 });
            AddItem(new Boots() { Hue = 1400 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

		public SirLancealot(Serial serial) : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Sir Lancelot, the most unappreciated knight in the realm. However, I've lost many memories since awakening in a wizard's tower... How may I assist you?");

            greeting.AddOption("What happened in the wizard's tower?",
                player => true,
                player => 
                {
                    DialogueModule towerModule = new DialogueModule("I woke up in a vat filled with strange liquids. The last thing I remember is... it's all a blur. Shadows danced at the edge of my vision. What sort of magic could have done this?");
                    towerModule.AddOption("What do you remember about the tower?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule memoryModule = new DialogueModule("There were arcane symbols on the walls and a strange, echoing voice that spoke of destinies and choices. I tried to grasp the meaning, but it slipped away like sand through my fingers.");
                            memoryModule.AddOption("Could it be linked to your past?",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule pastModule = new DialogueModule("Perhaps. But my past is shrouded in mystery. I feel like a ghost haunting my own life. Each time I grasp at a memory, it fades.");
                                    pastModule.AddOption("What do you wish to remember?",
                                        plaa => true,
                                        plaa => 
                                        {
                                            DialogueModule wishModule = new DialogueModule("I wish to recall the battles I fought, the friendships I forged, and why I was here in the first place. But all I have are fragmented visions of glory and pain.");
                                            wishModule.AddOption("What battles did you fight?",
                                                plaaa => true,
                                                plaaa => 
                                                {
                                                    DialogueModule battleModule = new DialogueModule("I remember flashes—clashing swords, roars of dragons, and the faces of comrades. But their names escape me. Was I a hero or a fool in those moments?");
                                                    battleModule.AddOption("What about your comrades?",
                                                        plaq => true,
                                                        plaq => 
                                                        {
                                                            DialogueModule comradeModule = new DialogueModule("I know I fought alongside valiant souls, but their faces are like mist. I can’t help but feel I’ve betrayed them by forgetting.");
                                                            comradeModule.AddOption("You need to find them.",
                                                                plw => true,
                                                                plw => 
                                                                {
                                                                    pl.SendMessage("Perhaps finding my comrades could lead to rediscovery.");
                                                                });
                                                            battleModule.AddOption("Do you think they remember you?",
                                                                plae => true,
                                                                plae => 
                                                                {
                                                                    Say("That thought torments me. What if they think I abandoned them?");
                                                                });
                                                            player.SendGump(new DialogueGump(player, comradeModule));
                                                        });
                                                    player.SendGump(new DialogueGump(player, battleModule));
                                                });
                                            player.SendGump(new DialogueGump(player, wishModule));
                                        });
                                    player.SendGump(new DialogueGump(player, pastModule));
                                });
                            player.SendGump(new DialogueGump(player, memoryModule));
                        });
                    player.SendGump(new DialogueGump(player, towerModule));
                });

            greeting.AddOption("What else do you remember?",
                player => true,
                player =>
                {
                    DialogueModule memoryFragmentModule = new DialogueModule("Only fragments. I recall a great hall, laughter, and a banquet. But the faces are blurred, and the names are lost. Was I a knight then? A lord?");
                    memoryFragmentModule.AddOption("What did you feel during those moments?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule feelingsModule = new DialogueModule("I felt joy, camaraderie, but also a creeping dread. Something was coming—something dark. But I cannot recall what it was. I wonder if it had to do with my fate in that tower.");
                            feelingsModule.AddOption("Do you fear returning to that tower?",
                                pla => true,
                                pla => 
                                {
                                    Say("I fear what I might discover. But I also feel a pull, a need to uncover the truth.");
                                });
                            feelingsModule.AddOption("What if it helps you regain your memory?",
                                pla => true,
                                pla => 
                                {
                                    Say("The hope of recovering lost pieces of my soul is enticing. Perhaps it's worth the risk.");
                                });
                            player.SendGump(new DialogueGump(player, feelingsModule));
                        });
                    player.SendGump(new DialogueGump(player, memoryFragmentModule));
                });

            greeting.AddOption("Why do you feel unappreciated?",
                player => true,
                player => 
                {
                    DialogueModule unappreciatedModule = new DialogueModule("Despite my efforts to protect this town, few recognize my contributions. My valor seems to be whispered about but never celebrated.");
                    unappreciatedModule.AddOption("Have you tried talking to the townsfolk?",
                        pl => true,
                        pl => 
                        {
                            Say("I’ve tried, but they seem to brush me aside as if I were just a relic of a forgotten time.");
                        });
                    unappreciatedModule.AddOption("What would you like them to know?",
                        pl => true,
                        pl => 
                        {
                            Say("I want them to understand that a knight’s duty is not merely to battle, but to stand as a bulwark against despair.");
                        });
                    player.SendGump(new DialogueGump(player, unappreciatedModule));
                });

            greeting.AddOption("Do you sell anything?",
                player => true,
                player => 
                {
                    DialogueModule shopModule = new DialogueModule("I have some items I’ve gathered in my travels. They might aid you on your quest.");
                    shopModule.AddOption("Let me see what you have.",
                        p => true,
                        p => 
                        {
                            p.SendMessage("Sir Lancelot shows you a collection of items for sale.");
                            // Open the player's buy/sell gump or shop interface
                        });
                    shopModule.AddOption("Maybe later.",
                        p => true,
                        p => 
                        {
                            Say("I’ll be here if you change your mind.");
                        });
                    player.SendGump(new DialogueGump(player, shopModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
