using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Celestial Seer")]
    public class LyriaTheCelestialSeer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaTheCelestialSeer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Celestial Seer";
            Body = 0x191; // Human female body

            // Stats
            SetStr(65);
            SetDex(70);
            SetInt(145);
            SetHits(85);

            // Appearance
            AddItem(new Robe() { Hue = 1177 }); // Robe with hue 1177
            AddItem(new Boots() { Hue = 1176 }); // Boots with hue 1176
            AddItem(new Cloak() { Name = "Lyria's Divination" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("Greetings, mortal. I am Lyria the Celestial Seer, a being far removed from your realm. How may I assist you?");

            greeting.AddOption("Tell me about your wisdom.",
                player => true,
                player => 
                {
                    DialogueModule wisdomModule = new DialogueModule("Ah, I see your curiosity remains undiminished. Do you seek wisdom or merely distractions?");
                    wisdomModule.AddOption("I seek wisdom.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateWisdomModule())));
                    wisdomModule.AddOption("I'm looking for distractions.",
                        pl => true,
                        pl =>
                        {
                            if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                            {
                                pl.SendMessage("I have no reward right now. Please return later.");
                            }
                            else
                            {
                                pl.SendMessage("Take this as a token of the unknown.");
                                pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            }
                        });
                    player.SendGump(new DialogueGump(player, wisdomModule));
                });

            greeting.AddOption("What is your role?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateRoleModule())));

            greeting.AddOption("Tell me about the cosmos.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateCosmosModule())));

            greeting.AddOption("Do you offer guidance?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGuidanceModule())));

            return greeting;
        }

        private DialogueModule CreateWisdomModule()
        {
            DialogueModule wisdomModule = new DialogueModule("Wisdom comes from understanding the balance of all things. What kind of wisdom do you seek?");
            wisdomModule.AddOption("How can I find my purpose?",
                pl => true,
                pl => 
                {
                    DialogueModule purposeModule = new DialogueModule("Your purpose is a journey, not a destination. Reflect on your passions and fears. What calls to you?");
                    purposeModule.AddOption("I want to help others.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateHelpModule())));
                    purposeModule.AddOption("I seek power and knowledge.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreatePowerModule())));
                    purposeModule.AddOption("I want to explore the world.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateExploreModule())));
                    pl.SendGump(new DialogueGump(pl, purposeModule));
                });

            wisdomModule.AddOption("What is the nature of reality?",
                pl => true,
                pl => 
                {
                    DialogueModule realityModule = new DialogueModule("Reality is but a canvas, painted with the colors of perception. Each being sees it differently. Do you wish to delve deeper?");
                    realityModule.AddOption("Yes, tell me more.",
                        p => true,
                        p => 
                        {
                            DialogueModule deeperRealityModule = new DialogueModule("Consider the layers of existence: physical, emotional, spiritual. They intertwine, shaping your experience.");
                            deeperRealityModule.AddOption("How do I balance these layers?",
                                pla => true,
                                pla => pl.SendGump(new DialogueGump(pl, CreateWisdomModule())));
                            deeperRealityModule.AddOption("This is overwhelming.",
                                plw => true,
                                plw => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, deeperRealityModule));
                        });
                    pl.SendGump(new DialogueGump(pl, realityModule));
                });

            return wisdomModule;
        }

        private DialogueModule CreateHelpModule()
        {
            return new DialogueModule("To help others is to understand their pain and joys. Seek out those in need, and lend a hand. You may discover your own strength in the process.");
        }

        private DialogueModule CreatePowerModule()
        {
            DialogueModule powerModule = new DialogueModule("Power comes with responsibility. Knowledge is a double-edged sword. How will you wield it?");
            powerModule.AddOption("I will use it for good.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateGoodUseModule())));
            powerModule.AddOption("I seek to dominate others.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateDominateModule())));
            return powerModule;
        }

        private DialogueModule CreateGoodUseModule()
        {
            return new DialogueModule("Then wield it wisely, and guide others to enlightenment. Together, you can achieve great things.");
        }

        private DialogueModule CreateDominateModule()
        {
            return new DialogueModule("Beware the path of tyranny. It leads to darkness. True strength lies in unity, not subjugation.");
        }

        private DialogueModule CreateExploreModule()
        {
            DialogueModule exploreModule = new DialogueModule("The world is filled with wonders and perils. What do you wish to discover?");
            exploreModule.AddOption("I want to explore ancient ruins.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateRuinsModule())));
            exploreModule.AddOption("I wish to journey through mystical forests.",
                p => true,
                p => p.SendGump(new DialogueGump(p, CreateForestModule())));
            return exploreModule;
        }

        private DialogueModule CreateRuinsModule()
        {
            return new DialogueModule("Ancient ruins hold the echoes of the past. They whisper secrets to those who listen. Tread carefully; the forgotten can be dangerous.");
        }

        private DialogueModule CreateForestModule()
        {
            return new DialogueModule("Mystical forests are alive with magic. Be attuned to your surroundings; nature often reveals truths to those who seek with an open heart.");
        }

        private DialogueModule CreateRoleModule()
        {
            return new DialogueModule("My role is to observe your world and offer cryptic guidance. I exist to bridge the realms of the celestial and the mundane.");
        }

        private DialogueModule CreateCosmosModule()
        {
            return new DialogueModule("The cosmos is vast, filled with energies and beings beyond your understanding. Your world is but a speck in its grand design, yet every speck has its purpose.");
        }

        private DialogueModule CreateGuidanceModule()
        {
            DialogueModule guidanceModule = new DialogueModule("I can offer guidance to those who truly seek knowledge. What kind of guidance do you require?");
            guidanceModule.AddOption("I seek guidance for my journey.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJourneyGuidanceModule())));
            guidanceModule.AddOption("What artifacts do you offer?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateArtifactsModule())));
            return guidanceModule;
        }

        private DialogueModule CreateJourneyGuidanceModule()
        {
            return new DialogueModule("Your journey is uniquely yours. Trust in your instincts and remain open to learning. Every encounter can teach you something invaluable.");
        }

        private DialogueModule CreateArtifactsModule()
        {
            DialogueModule artifactsModule = new DialogueModule("Artifacts from the celestial realm are infused with energies unknown to your kind. They can be a boon or a bane, depending on the wielder.");
            artifactsModule.AddOption("Can you share a story about an artifact?",
                player => true,
                player =>
                {
                    DialogueModule artifactStoryModule = new DialogueModule("Once, a mortal found an amulet that granted them immense power. However, they became consumed by it, losing their sense of self. Beware such temptations.");
                    artifactStoryModule.AddOption("Thank you for the warning.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, artifactStoryModule));
                });
            return artifactsModule;
        }

        public LyriaTheCelestialSeer(Serial serial) : base(serial) { }

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
