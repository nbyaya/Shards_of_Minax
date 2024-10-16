using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Star Explorer Leo")]
    public class StarExplorerLeo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public StarExplorerLeo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Star Explorer Leo";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 100;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new ChainLegs() { Hue = 2211 });
            AddItem(new ChainChest() { Hue = 2211 });
            AddItem(new PlateHelm() { Hue = 2211 });
            AddItem(new Boots() { Hue = 2211 });
            AddItem(new FireballWand() { Name = "Leo's Laser Gun" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public StarExplorerLeo(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Oh, great, another curious mortal... What brings you here?");
            
            greeting.AddOption("Tell me your name.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateNameModule())); });

            greeting.AddOption("What is your job?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

            greeting.AddOption("What is your purpose?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreatePurposeModule())); });

            greeting.AddOption("Tell me about the Encyclopedia Foundation.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoundationModule())); });

            greeting.AddOption("I have deeds to discuss.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDeedsModule())); });

            return greeting;
        }

        private DialogueModule CreateNameModule()
        {
            return new DialogueModule("I am Leo, a traveler of the stars. Now, what else would you like to know?");
        }

        private DialogueModule CreateJobModule()
        {
            return new DialogueModule("My 'job'? I'm here, talking to you, aren't I? But I also work with the Encyclopedia Foundation, seeking knowledge and rebuilding the Planar Imperium.");
        }

        private DialogueModule CreatePurposeModule()
        {
            DialogueModule purposeModule = new DialogueModule("True wisdom is not in asking pointless questions, but in making meaningful choices. My purpose is to explore the cosmos and uncover its secrets. What's your purpose here?");
            purposeModule.AddOption("I seek knowledge.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateKnowledgeModule())); });

            return purposeModule;
        }

        private DialogueModule CreateKnowledgeModule()
        {
            return new DialogueModule("Knowledge is the key to understanding the universe. Be wise in your pursuits.");
        }

        private DialogueModule CreateFoundationModule()
        {
            DialogueModule foundationModule = new DialogueModule("The Encyclopedia Foundation is dedicated to preserving knowledge and restoring the lost civilizations of the Planar Imperium. They gather scholars and adventurers alike.");
            foundationModule.AddOption("What is the Planar Imperium?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateImperiumModule())); });

            foundationModule.AddOption("How can I help the Foundation?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateHelpModule())); });

            return foundationModule;
        }

        private DialogueModule CreateImperiumModule()
        {
            DialogueModule imperiumModule = new DialogueModule("The Planar Imperium was once a grand civilization that spanned multiple realms, filled with wonders and powerful beings. Its fall left a void in the fabric of reality, and we strive to restore it.");
            imperiumModule.AddOption("What caused its downfall?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDownfallModule())); });

            imperiumModule.AddOption("What can be done to restore it?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateRestoreModule())); });

            return imperiumModule;
        }

        private DialogueModule CreateDownfallModule()
        {
            return new DialogueModule("The downfall was due to a combination of internal strife, greed, and a catastrophic event known as the Shattering. This event tore apart the very essence of the Imperium, scattering its knowledge and power.");
        }

        private DialogueModule CreateRestoreModule()
        {
            DialogueModule restoreModule = new DialogueModule("To restore the Planar Imperium, we must gather ancient artifacts, scrolls, and relics of power. These items hold the key to understanding our past and rebuilding what was lost.");
            restoreModule.AddOption("What artifacts do you seek?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateArtifactsModule())); });

            return restoreModule;
        }

        private DialogueModule CreateArtifactsModule()
        {
            return new DialogueModule("We seek items such as the Celestial Tome, the Shard of the Cosmos, and the Crown of the Astral Kings. Each of these has its own lore and significance, crucial to our efforts.");
        }

        private DialogueModule CreateHelpModule()
        {
            DialogueModule helpModule = new DialogueModule("You can help by embarking on quests to find these lost artifacts and sharing any knowledge you uncover. Every piece of information can aid us in our mission.");
            helpModule.AddOption("What quests do you have in mind?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateQuestsModule())); });

            return helpModule;
        }

        private DialogueModule CreateQuestsModule()
        {
            DialogueModule questsModule = new DialogueModule("One quest involves exploring the ruins of the old Imperium, seeking the Celestial Tome. Another requires venturing into the Whispering Forest to uncover the Shard of the Cosmos.");
            questsModule.AddOption("I'll take on these quests!",
                player => true,
                player =>
                {
                    player.SendMessage("Leo smiles. 'Your courage is commendable! May the stars guide you on your journey.'");
                    // Here you could initiate quest logic if needed
                });

            questsModule.AddOption("That sounds too dangerous.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoundationModule())); });

            return questsModule;
        }

        private DialogueModule CreateDeedsModule()
        {
            DialogueModule deedsModule = new DialogueModule("A hero is defined not by their title, but by their actions. Do something truly noble, and I'll ensure you're appropriately rewarded.");

            deedsModule.AddOption("What kind of deeds?",
                player => true,
                player =>
                {
                    DialogueModule rewardModule = new DialogueModule("A noble deed could be anything from helping the weak to defeating a great foe. Prove your worth, and a reward awaits!");
                    rewardModule.AddOption("I shall return with my deeds.",
                        pl => true,
                        pl =>
                        {
                            if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                            {
                                pl.SendMessage("I have no reward for you right now. Please return later.");
                            }
                            else
                            {
                                pl.AddToBackpack(new MaxxiaScroll()); // Example reward
                                lastRewardTime = DateTime.UtcNow; // Update timestamp
                                pl.SendMessage("You have been rewarded for your deeds.");
                            }
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                });

            return deedsModule;
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
