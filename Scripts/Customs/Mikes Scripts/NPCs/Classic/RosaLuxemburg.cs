using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rosa Luxemburg")]
    public class RosaLuxemburg : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RosaLuxemburg() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rosa Luxemburg";
            Body = 0x191; // Human female body

            // Stats
            SetStr(85);
            SetDex(70);
            SetInt(95);
            SetHits(74);

            // Appearance
            AddItem(new Skirt() { Hue = 1912 });
            AddItem(new PlainDress() { Hue = 1104 });
            AddItem(new Boots() { Hue = 1104 });
            AddItem(new WarFork() { Name = "Revolutionary's Spear" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
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
            DialogueModule greeting = new DialogueModule("I am Rosa Luxemburg, a champion of the proletariat! We stand at the cusp of a revolution against Lord British's oppressive rule. Are you ready to discuss the path toward true freedom?");
            
            greeting.AddOption("What do you mean by oppressive rule?",
                player => true,
                player =>
                {
                    DialogueModule oppressiveModule = new DialogueModule("Lord British's reign has suffocated our voices and shackled our ambitions. The nobility hoards power, while the common folk suffer. It is time to dismantle this corrupt system!");
                    oppressiveModule.AddOption("How can we dismantle this system?",
                        p => true,
                        p =>
                        {
                            DialogueModule dismantleModule = new DialogueModule("We must unite! Through collective action and the power of the masses, we can overthrow the tyrant and build a society where all are free to flourish!");
                            dismantleModule.AddOption("And what about Lord Blackthorn?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule blackthornModule = new DialogueModule("Lord Blackthorn embraces the philosophy of Anarchism, advocating for a society free from hierarchies. He sees the value in individual liberty, and we must learn from his vision.");
                                    blackthornModule.AddOption("Is Blackthorn truly different from British?",
                                        pq => true,
                                        pq =>
                                        {
                                            DialogueModule differenceModule = new DialogueModule("While both may have held power, Blackthorn seeks to dismantle the oppressive structures that British upholds. He dreams of a world where authority is decentralized and every voice matters.");
                                            differenceModule.AddOption("What can I do to support this vision?",
                                                pla => true,
                                                pla =>
                                                {
                                                    DialogueModule supportModule = new DialogueModule("You can start by spreading awareness and gathering support among the people. Encourage others to join the movement for change! Our strength lies in our unity.");
                                                    supportModule.AddOption("I want to join the movement!",
                                                        plw => true,
                                                        plw =>
                                                        {
                                                            HandleThriveOption(pl);
                                                        });
                                                    supportModule.AddOption("I need to think about it.",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    pla.SendGump(new DialogueGump(pla, supportModule));
                                                });
                                            p.SendGump(new DialogueGump(p, differenceModule));
                                        });
                                    player.SendGump(new DialogueGump(player, blackthornModule));
                                });
                            player.SendGump(new DialogueGump(player, dismantleModule));
                        });
                    player.SendGump(new DialogueGump(player, oppressiveModule));
                });

            greeting.AddOption("What role do the workers play in this revolution?",
                player => true,
                player =>
                {
                    DialogueModule workersModule = new DialogueModule("The workers are the backbone of any society! Without our labor, the elite cannot thrive. When we rise together, we reclaim our power!");
                    workersModule.AddOption("But how do we organize ourselves?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule organizeModule = new DialogueModule("We must form councils and local assemblies to represent our collective interests. Communication and solidarity are key! Only then can we effect real change.");
                            organizeModule.AddOption("What if the nobility tries to suppress us?",
                                p => true,
                                p =>
                                {
                                    DialogueModule suppressModule = new DialogueModule("They will try, but we are many and they are few. Through perseverance and our shared determination, we can stand against their oppression!");
                                    suppressModule.AddOption("I'm inspired! What should I do next?",
                                        pla => true,
                                        pla =>
                                        {
                                            HandleThriveOption(pla);
                                        });
                                    p.SendGump(new DialogueGump(p, suppressModule));
                                });
                            pl.SendGump(new DialogueGump(pl, organizeModule));
                        });
                    player.SendGump(new DialogueGump(player, workersModule));
                });

            greeting.AddOption("Is violence necessary for revolution?",
                player => true,
                player =>
                {
                    DialogueModule violenceModule = new DialogueModule("Revolution often comes with struggle, but it is not the first course of action we should take. Our aim should be to educate and inspire, but we must be prepared to defend ourselves if necessary.");
                    violenceModule.AddOption("I see your point. How do we educate the masses?",
                        p => true,
                        p =>
                        {
                            DialogueModule educateModule = new DialogueModule("We can spread pamphlets, organize gatherings, and engage in discussions with our fellow workers. Knowledge is a powerful tool for liberation!");
                            educateModule.AddOption("I will help spread the message!",
                                pla => true,
                                pla =>
                                {
                                    HandleThriveOption(pla);
                                });
                            p.SendGump(new DialogueGump(p, educateModule));
                        });
                    player.SendGump(new DialogueGump(player, violenceModule));
                });

            greeting.AddOption("Tell me more about Lord Blackthorn's vision.",
                player => true,
                player =>
                {
                    DialogueModule blackthornVisionModule = new DialogueModule("Blackthorn envisions a society where individuals are empowered to govern themselves without the constraints of authoritarian rule. He believes in the inherent goodness of people!");
                    blackthornVisionModule.AddOption("That sounds appealing. How do we achieve that?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule achieveModule = new DialogueModule("We must cultivate trust among each other and build systems that prioritize cooperation and mutual aid. It begins with us!");
                            achieveModule.AddOption("I'm ready to contribute to this vision!",
                                pla => true,
                                pla =>
                                {
                                    HandleThriveOption(pla);
                                });
                            pl.SendGump(new DialogueGump(pl, achieveModule));
                        });
                    player.SendGump(new DialogueGump(player, blackthornVisionModule));
                });

            greeting.AddOption("What do you think will happen if we succeed?",
                player => true,
                player =>
                {
                    DialogueModule successModule = new DialogueModule("If we succeed, we will create a world where everyone has a voice, where resources are shared equitably, and where creativity flourishes unencumbered by tyranny. It will be a beautiful world!");
                    successModule.AddOption("That sounds wonderful! How can I help?",
                        p => true,
                        p =>
                        {
                            HandleThriveOption(p);
                        });
                    player.SendGump(new DialogueGump(player, successModule));
                });

            return greeting;
        }

        private void HandleThriveOption(PlayerMobile player)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                player.SendMessage("I have no reward right now. Please return later.");
            }
            else
            {
                player.SendMessage("Your commitment to the cause is commendable! Here, take this as a token of gratitude for your willingness to fight alongside us.");
                player.AddToBackpack(new DetectingHiddenAugmentCrystal()); // Replace with the actual reward item
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            player.SendGump(new DialogueGump(player, CreateGreetingModule()));
        }

        public RosaLuxemburg(Serial serial) : base(serial) { }

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
