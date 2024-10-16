using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ulf the Frostborn")]
    public class UlfTheFrostborn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public UlfTheFrostborn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ulf the Frostborn";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 90;
            Int = 30;
            Hits = 100;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1154 });
            AddItem(new StuddedChest() { Hue = 1154 });
            AddItem(new StuddedGloves() { Hue = 1154 });
            AddItem(new VikingSword() { Name = "Ulf's Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
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
            DialogueModule greeting = new DialogueModule("Ulf the Frostborn, they call me. What brings you to these icy lands?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => { player.SendMessage("Health? Bah! A scratch!"); });

            greeting.AddOption("What is your job?",
                player => true,
                player => { player.SendMessage("My job? Surviving in this wretched world!"); });

            greeting.AddOption("Are you tougher than me?",
                player => true,
                player => { player.SendMessage("Life here is harsh, stranger. Are you made of tougher stuff?"); });

            greeting.AddOption("What's your greatest battle?",
                player => true,
                player =>
                {
                    DialogueModule battleModule = new DialogueModule("If you're as tough as you claim, prove it! Tell me, stranger, what's your greatest battle?");
                    battleModule.AddOption("Let me tell you...",
                        pl => true,
                        pl =>
                        {
                            if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                            {
                                pl.AddToBackpack(new MaxxiaScroll());
                                lastRewardTime = DateTime.UtcNow;
                                pl.SendMessage("Ulf grants you a token from his clan. May it serve you well.");
                            }
                            else
                            {
                                pl.SendMessage("I have no reward right now. Please return later.");
                            }
                        });
                    player.SendGump(new DialogueGump(player, battleModule));
                });

            greeting.AddOption("Tell me about the Frostborn.",
                player => true,
                player => { player.SendMessage("They named me Frostborn because I was discovered as a babe in the heart of a snowstorm, left untouched by its fury."); });

            greeting.AddOption("What do you know about dire wolves?",
                player => true,
                player => { player.SendMessage("Those dire wolves are a menace, but they also hold a deep connection to our land. Some say they guard the spirits of our ancestors."); });

            greeting.AddOption("What relics do you seek?",
                player => true,
                player => { player.SendMessage("These relics are said to hold immense power. Many have tried to find them, but the icy wilderness is unforgiving."); });

            greeting.AddOption("What can you tell me about the gods?",
                player => true,
                player => { player.SendMessage("The gods of the north are old and powerful. We pay our respects to them through rituals and offerings."); });

            greeting.AddOption("What was it like growing up in the frost lands?",
                player => true,
                player =>
                {
                    DialogueModule childhoodModule = new DialogueModule("Ah, growing up in the frost lands is both a blessing and a curse. The beauty of the snow-covered mountains hides many dangers.");
                    childhoodModule.AddOption("What dangers did you face?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangersModule = new DialogueModule("From ferocious beasts to treacherous weather, every day was a test. The cold bites deeper than any blade.");
                            dangersModule.AddOption("Tell me about the beasts.",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Wolves and bears stalk the night, while the icy winds carry the howls of hungry spirits. We learn to respect our foes.");
                                });
                            dangersModule.AddOption("What about the weather?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("The storms can come without warning, burying whole villages under mountains of snow. One must always be prepared.");
                                });
                            dangersModule.AddOption("How did you survive?",
                                p => true,
                                p =>
                                {
                                    DialogueModule survivalModule = new DialogueModule("Survival is a lesson taught by the land itself. We learned to hunt, fish, and gather what little warmth we could find.");
                                    survivalModule.AddOption("What do you hunt?",
                                        plq => true,
                                        plq => { pl.SendMessage("Deer, hares, and even the occasional bear, if one is brave enough. Each hunt teaches us something new."); });
                                    survivalModule.AddOption("Do you fish in the frozen lakes?",
                                        plw => true,
                                        plw => { pl.SendMessage("Yes, we carve holes in the ice and wait patiently for a bite. Patience is a virtue learned in the frost."); });
                                    survivalModule.AddOption("And what do you gather?",
                                        ple => true,
                                        ple => { pl.SendMessage("Berries in the summer, herbs that brave the cold, and the occasional treasure from the depths of the snow."); });
                                    p.SendGump(new DialogueGump(p, survivalModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dangersModule));
                        });
                    childhoodModule.AddOption("What about your family?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule familyModule = new DialogueModule("Family is everything in the frost lands. We rely on each other for warmth and strength. My clan was my lifeline.");
                            familyModule.AddOption("What about your clan?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("The Frostborn clan is resilient. We gather by the fire, share stories, and pass down our history.");
                                });
                            familyModule.AddOption("What traditions do you have?",
                                p => true,
                                p =>
                                {
                                    DialogueModule traditionsModule = new DialogueModule("We have many rituals, especially during the solstice. We honor the spirits of the land and seek their blessings.");
                                    traditionsModule.AddOption("What rituals do you perform?",
                                        plr => true,
                                        plr => { pl.SendMessage("Offerings of food, dances under the stars, and storytelling around the fire are our ways of connecting with the spirits."); });
                                    traditionsModule.AddOption("Do you believe in spirits?",
                                        plt => true,
                                        plt => { pl.SendMessage("Absolutely. The frost lands are alive with their presence, guiding and protecting us in their own way."); });
                                    p.SendGump(new DialogueGump(p, traditionsModule));
                                });
                            player.SendGump(new DialogueGump(player, familyModule));
                        });
                    player.SendGump(new DialogueGump(player, childhoodModule));
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

        public UlfTheFrostborn(Serial serial) : base(serial) { }
    }
}
