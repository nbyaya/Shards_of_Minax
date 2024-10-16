using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Lancelot")]
    public class SirLancelot : BaseCreature
    {
        [Constructable]
        public SirLancelot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Lancelot";
            Body = 0x190; // Human male body

            // Stats
            SetStr(158);
            SetDex(66);
            SetInt(26);
            SetHits(113);

            // Appearance
            AddItem(new ChainChest() { Hue = 1400 });
            AddItem(new ChainLegs() { Hue = 1400 });
            AddItem(new PlateHelm() { Hue = 1400 });
            AddItem(new PlateGloves() { Hue = 1400 });
            AddItem(new Boots() { Hue = 1400 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public SirLancelot(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Sir Lancelot, a knight of noble lineage. How may I assist you?");

            greeting.AddOption("Tell me about your encounter with an evil wizard.",
                player => true,
                player => 
                {
                    DialogueModule wizardEncounter = new DialogueModule("Ah, it was a dark day when I faced that foul wizard. He had a terrible power, and I found myself trapped in a cloning vat of his own making.");
                    wizardEncounter.AddOption("What happened during that encounter?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule encounterDetails = new DialogueModule("The wizard sought to steal my strength and valor. I fought valiantly, but he overwhelmed me with dark magic, casting me into a vat that twisted my very essence.");
                            encounterDetails.AddOption("How did you escape?",
                                plq => true,
                                plq => 
                                {
                                    DialogueModule escapeStory = new DialogueModule("In the depths of despair, I recalled the teachings of my mentors. Through sheer will, I summoned the strength to shatter the vat and escape its grasp.");
                                    escapeStory.AddOption("What did you learn from this experience?",
                                        plw => true,
                                        plw => 
                                        {
                                            player.SendGump(new DialogueGump(player, new DialogueModule("I learned that true strength lies not in one's body, but in the heart and mind. Every challenge can be overcome with courage and wisdom.")));
                                        });
                                    player.SendGump(new DialogueGump(player, escapeStory));
                                });
                            encounterDetails.AddOption("What did the wizard look like?",
                                ple => true,
                                ple => 
                                {
                                    player.SendGump(new DialogueGump(player, new DialogueModule("The wizard was cloaked in shadows, with eyes that glowed like embers. His voice chilled the very air, and his magic was a palpable darkness.")));
                                });
                            player.SendGump(new DialogueGump(player, encounterDetails));
                        });
                    wizardEncounter.AddOption("What is a cloning vat?",
                        pl => true,
                        pl => 
                        {
                            player.SendGump(new DialogueGump(player, new DialogueModule("A foul device, it was! It could replicate living beings, trapping their essence within. It is said to be a creation of twisted minds who seek to play god.")));
                        });
                    player.SendGump(new DialogueGump(player, wizardEncounter));
                });

            greeting.AddOption("What is your health like?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My health is robust, as befits a knight of the realm.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I serve the kingdom as a protector and defender.")));
                });

            greeting.AddOption("What is valor?",
                player => true,
                player =>
                {
                    DialogueModule valorModule = new DialogueModule("True valor lies not only in the strength of one's arm but in the purity of one's heart. Dost thou value valor?");
                    valorModule.AddOption("Yes.",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Then thou art on the path of honor. A true knight never wavers in the face of adversity.")));
                        });
                    valorModule.AddOption("No.",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Then perhaps you should reflect on your values.")));
                        });
                    player.SendGump(new DialogueGump(player, valorModule));
                });

            greeting.AddOption("What can you tell me about your lineage?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My lineage traces back to the ancient knights who served the realm with honor and courage.")));
                });

            greeting.AddOption("How do you maintain your resilience?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Being robust isn't solely about physical strength, but also mental resilience. I often meditate on the mantra of Sacrifice to help me maintain both.")));
                });

            greeting.AddOption("What have you learned from your quests?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Every peril I've faced has taught me a lesson. There's always a glimmer of hope and a lesson to be learned.")));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
