using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ragnar the Bold")]
    public class RagnarTheBold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RagnarTheBold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ragnar the Bold";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(90);
            SetInt(60);
            SetHits(90);

            // Appearance
            AddItem(new ChainChest() { Name = "Ragnar's Chain Tunic" });
            AddItem(new ChainLegs() { Name = "Ragnar's Chain Leggings" });
            AddItem(new ChainCoif() { Name = "Ragnar's Chain Coif" });
            AddItem(new PlateGloves() { Name = "Ragnar's Plate Gloves" });
            AddItem(new Boots() { Name = "Ragnar's Boots" });
            AddItem(new Crossbow() { Name = "Ragnar's Crossbow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("I am Ragnar the Bold, the master archer! What brings you to my presence, traveler?");
            
            greeting.AddOption("Tell me about your love for soft furs.",
                player => true,
                player =>
                {
                    DialogueModule fursModule = new DialogueModule("Ah, soft furs! They are a treasure, indeed. The gentle touch of fur against the skin provides warmth and comfort like no other. Would you like to hear more?");
                    
                    fursModule.AddOption("Yes, please!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule detailsModule = new DialogueModule("Furs have been a part of my life since I was a child. I remember my first fur cloak, a gift from my father. It was made from the softest rabbit fur, and it felt like a warm embrace.");
                            
                            detailsModule.AddOption("What do you love most about furs?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule loveModule = new DialogueModule("The softness, of course! There's something magical about wearing a piece of nature that feels so inviting. It reminds me of the peaceful moments spent in the forest, wrapped in nature's embrace.");
                                    
                                    loveModule.AddOption("Do you have a favorite type of fur?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule favoriteModule = new DialogueModule("Ah, the most exquisite fur I’ve encountered must be that of the Arctic Fox. Its soft, silken texture is unparalleled, and it shines like moonlight on snow.");
                                            
                                            favoriteModule.AddOption("Have you ever hunted for it?",
                                                ple => true,
                                                ple =>
                                                {
                                                    DialogueModule huntModule = new DialogueModule("Indeed! Hunting for the Arctic Fox was a thrilling experience. They are elusive creatures, but with patience and skill, I managed to track one down. The fur I acquired was truly a prize.");
                                                    
                                                    huntModule.AddOption("What did you do with the fur?",
                                                        plr => true,
                                                        plr =>
                                                        {
                                                            DialogueModule useModule = new DialogueModule("I crafted it into a beautiful cloak, adorned with intricate patterns that symbolize the forest's spirit. I wear it during ceremonies to honor nature.");
                                                            
                                                            useModule.AddOption("Can you show me?",
                                                                plt => true,
                                                                plt =>
                                                                {
                                                                    pl.SendMessage("Ragnar proudly displays the cloak, its elegance and beauty shining in the light.");
                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                });
                                                            
                                                            useModule.AddOption("Sounds incredible! Can I have one?",
                                                                ply => true,
                                                                ply =>
                                                                {
                                                                    DialogueModule offerModule = new DialogueModule("Ah, the crafting of such a cloak requires great skill and rare materials. If you bring me the right furs, perhaps I could make you one!");
                                                                    offerModule.AddOption("What materials do you need?",
                                                                        pla => true,
                                                                        pla =>
                                                                        {
                                                                            DialogueModule materialsModule = new DialogueModule("I would need the fur of an Arctic Fox, some Bear Hide for strength, and a bit of silk from the Forest Spiders. It would be a journey, but the result would be magnificent.");
                                                                            materialsModule.AddOption("I'll gather them for you.",
                                                                                p => true,
                                                                                p =>
                                                                                {
                                                                                    p.SendMessage("You have accepted the quest to gather the materials!");
                                                                                    p.AddToBackpack(new MaxxiaScroll()); // Simulated quest item
                                                                                    lastRewardTime = DateTime.UtcNow; // Update last reward time
                                                                                });
                                                                            materialsModule.AddOption("That sounds too challenging for me.",
                                                                                plu => true,
                                                                                plu =>
                                                                                {
                                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                                });
                                                                            pla.SendGump(new DialogueGump(pla, materialsModule));
                                                                        });
                                                                    player.SendGump(new DialogueGump(player, offerModule));
                                                                });
                                                            
                                                            pl.SendGump(new DialogueGump(pl, useModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, huntModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, favoriteModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, loveModule));
                                });
                            pl.SendGump(new DialogueGump(pl, detailsModule));
                        });
                    fursModule.AddOption("No, thank you.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, fursModule));
                });

            greeting.AddOption("Do you have any quests for me?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        DialogueModule rewardModule = new DialogueModule("I have no reward right now. Please return later.");
                        rewardModule.AddOption("I understand.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, rewardModule));
                    }
                    else
                    {
                        DialogueModule questModule = new DialogueModule("Many seek my guidance, and in return, I often set them on a quest to test their skills and determination. If you prove yourself worthy, I might just have a special reward for you.");
                        questModule.AddOption("What must I do?",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Ragnar gives you a quest.");
                                pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, questModule));
                    }
                });

            greeting.AddOption("Tell me about the forest.",
                player => true,
                player =>
                {
                    DialogueModule forestModule = new DialogueModule("The forest is my sanctuary. It's where I find peace and solace. The rustling leaves, the chirping birds, they all speak to me.");
                    forestModule.AddOption("What do you love most about it?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loveForestModule = new DialogueModule("The forest provides everything—food, shelter, and the softest furs to wear! It's a treasure trove of resources.");
                            loveForestModule.AddOption("Have you ever had an adventure in the forest?",
                                p => true,
                                p =>
                                {
                                    DialogueModule adventureModule = new DialogueModule("Oh, countless! One time, I encountered a family of bears. They were majestic creatures, and I felt a deep respect for them. I left without taking a single fur, for it was not my place to disturb them.");
                                    adventureModule.AddOption("What happened next?",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendMessage("Ragnar recounts his adventure, filling your ears with tales of nature's beauty.");
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, adventureModule));
                                });
                            loveForestModule.AddOption("I can understand that.",
                                pli => true,
                                pli =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, loveForestModule));
                        });
                    player.SendGump(new DialogueGump(player, forestModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Ragnar nods and bids you farewell.");
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

        public RagnarTheBold(Serial serial) : base(serial) { }
    }
}
