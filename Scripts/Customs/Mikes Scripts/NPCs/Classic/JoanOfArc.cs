using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Joan of Arc")]
    public class JoanOfArc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JoanOfArc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Joan of Arc";
            Body = 0x191; // Human female body

            // Stats
            Str = 140;
            Dex = 40;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 38 });
            AddItem(new PlateChest() { Hue = 38 });
            AddItem(new PlateHelm() { Hue = 38 });
            AddItem(new PlateGloves() { Hue = 38 });
            AddItem(new PlateArms() { Hue = 38 });
            AddItem(new PlateGorget() { Hue = 38 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new Halberd() { Name = "Lady Joan of Arc's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule(player);
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule(PlayerMobile player)
        {
            DialogueModule greeting = new DialogueModule("I am Joan of Arc, the Maid of Orléans! How may I assist you, brave soul?");

            greeting.AddOption("Tell me about your health.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateHealthModule())));

            greeting.AddOption("What is your job?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateJobModule())));

            greeting.AddOption("What is divine destiny?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateDivineDestinyModule())));

            greeting.AddOption("Tell me about Orléans.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateOrleansModule())));

            greeting.AddOption("Can you tell me about the relic?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateRelicModule(player))));

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            return new DialogueModule("My health is of no concern, for I am guided by a higher purpose. Yet, I must remain vigilant.");
        }

        private DialogueModule CreateJobModule()
        {
            DialogueModule module = new DialogueModule("I am on a divine mission to free France from its oppressors! Would you like to know how I began this journey?");
            module.AddOption("Yes, how did you begin?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateJourneyStartModule())));
            module.AddOption("No, I wish to hear more about your current mission.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateCurrentMissionModule())));
            return module;
        }

        private DialogueModule CreateJourneyStartModule()
        {
            return new DialogueModule("It all began when I was but a girl, tending to my family's sheep. I heard the voices of angels calling me to save my country.");
        }

        private DialogueModule CreateCurrentMissionModule()
        {
            DialogueModule module = new DialogueModule("I currently rally the troops and inspire them with hope. Every battle fought is a step towards our freedom.");
            module.AddOption("What battles have you fought?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateBattlesModule())));
            return module;
        }

        private DialogueModule CreateBattlesModule()
        {
            DialogueModule module = new DialogueModule("Each battle is fierce, but we have won many. The Siege of Orléans was a pivotal moment!");
            module.AddOption("Tell me more about the Siege of Orléans.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateSiegeDetailsModule())));
            return module;
        }

        private DialogueModule CreateSiegeDetailsModule()
        {
            return new DialogueModule("We fought bravely against the English and broke their siege. It was a victory for all of France!");
        }

        private DialogueModule CreateDivineDestinyModule()
        {
            DialogueModule module = new DialogueModule("The voices of angels guide my path. Do you believe in divine destiny?");
            module.AddOption("Yes",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreatePositiveResponseModule())));
            module.AddOption("No",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateNegativeResponseModule())));
            return module;
        }

        private DialogueModule CreatePositiveResponseModule()
        {
            return new DialogueModule("Your answer reveals much about your character. Proceed with courage, my friend.");
        }

        private DialogueModule CreateNegativeResponseModule()
        {
            return new DialogueModule("Doubt can cloud one's purpose. Stay strong in your beliefs, for faith can move mountains.");
        }

        private DialogueModule CreateOrleansModule()
        {
            DialogueModule module = new DialogueModule("Orléans was a significant victory for France, showing that through unity and faith, we could triumph over adversity.");
            module.AddOption("What does Orléans mean to you?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateOrleansMeaningModule())));
            return module;
        }

        private DialogueModule CreateOrleansMeaningModule()
        {
            return new DialogueModule("It means hope and resilience. A reminder that even in our darkest hours, we can prevail.");
        }

        private DialogueModule CreateRelicModule(PlayerMobile player)
        {
            DialogueModule module = new DialogueModule("There's an ancient relic, lost to the sands of time, that could aid our cause. If you could find it, I'd be in your debt.");
            module.AddOption("What is this relic?",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateRelicDescriptionModule())));
            module.AddOption("I'll find it for you.",
                p => true,
                p => GiveRewardIfNeeded(p));
            return module;
        }

        private DialogueModule CreateRelicDescriptionModule()
        {
            return new DialogueModule("The relic is said to possess great power, able to inspire our soldiers and turn the tide of battle!");
        }

        private void GiveRewardIfNeeded(PlayerMobile player)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                player.SendMessage("I have no reward right now. Please return later.");
            }
            else
            {
                player.SendMessage("You have my gratitude for assisting in this noble quest. Accept this reward.");
                player.AddToBackpack(new TacticsAugmentCrystal()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
        }

        public JoanOfArc(Serial serial) : base(serial) { }

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
