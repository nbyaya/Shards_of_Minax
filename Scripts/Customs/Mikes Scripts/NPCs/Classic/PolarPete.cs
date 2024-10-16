using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Polar Pete")]
    public class PolarPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PolarPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Polar Pete";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(160);

            // Appearance
            AddItem(new FurCape() { Hue = 1173 });
            AddItem(new FurBoots() { Hue = 1173 });
            AddItem(new FurSarong() { Hue = 1173 });
            AddItem(new Mace() { Name = "Polar Pete's Mace" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public PolarPete(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Polar Pete, the animal tamer. My heart belongs to the snow dogs I raise in these icy lands. How can I assist you today?");

            greeting.AddOption("Tell me about your snow dogs.",
                player => true,
                player => 
                {
                    DialogueModule dogsModule = new DialogueModule("Ah, my beloved snow dogs! They're more than just animals to me; they're family. Their loyalty and strength are unmatched. Would you like to hear about their training?");
                    dogsModule.AddOption("Yes, please tell me about their training.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule trainingModule = new DialogueModule("Training snow dogs requires patience and understanding. I start when they're just pups, teaching them basic commands like 'sit' and 'stay'. They learn quickly, especially with a little encouragement.");
                            trainingModule.AddOption("What commands do they learn?",
                                p => true,
                                p =>
                                {
                                    DialogueModule commandsModule = new DialogueModule("They learn essential commands such as 'come', 'heel', and 'fetch'. Each command strengthens our bond and makes them reliable companions during long treks through the snow.");
                                    commandsModule.AddOption("Do they enjoy training?",
                                        plq => true,
                                        plq => 
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Absolutely! They thrive on the mental stimulation and physical activity. It's a joy to see their enthusiasm when they're learning something new.")));
                                        });
                                    commandsModule.AddOption("What if they misbehave?",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("If they misbehave, I remain calm and redirect their energy. It's important to understand their perspective. After all, they're curious creatures!" +
                                                " Consistent reinforcement helps correct their behavior.")));
                                        });
                                    p.SendGump(new DialogueGump(p, commandsModule));
                                });
                            trainingModule.AddOption("Do you have any special training methods?",
                                ple => true,
                                ple =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("I often use positive reinforcement techniques, rewarding them with treats or affection when they succeed. This creates a joyful learning environment.")));
                                });
                            pl.SendGump(new DialogueGump(pl, trainingModule));
                        });
                    dogsModule.AddOption("What breeds do you raise?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule breedsModule = new DialogueModule("I primarily raise Siberian Huskies and Alaskan Malamutes. Both breeds have incredible stamina and adaptability to the harsh Arctic conditions.");
                            breedsModule.AddOption("What are their characteristics?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Siberian Huskies are known for their friendly nature and playful spirit, while Alaskan Malamutes are strong and dignified. Each has unique traits that make them special companions.")));
                                });
                            breedsModule.AddOption("What do you feed them?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Their diet mainly consists of high-protein dog food, mixed with meat and fish when I can afford it. In winter, they need extra calories to keep warm.")));
                                });
                            pl.SendGump(new DialogueGump(pl, breedsModule));
                        });
                    player.SendGump(new DialogueGump(player, dogsModule));
                });

            greeting.AddOption("How do you raise them in the Arctic?",
                player => true,
                player => 
                {
                    DialogueModule raisingModule = new DialogueModule("Raising snow dogs in the Arctic requires a deep understanding of their needs. I provide them with insulated shelters and regular exercise. It's crucial to keep them healthy and happy.");
                    raisingModule.AddOption("What kind of shelter do they have?",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I build insulated kennels that protect them from the biting winds and harsh cold. They also have access to open space to run and play.")));
                        });
                    raisingModule.AddOption("How do you keep them healthy?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule healthModule = new DialogueModule("Regular veterinary check-ups are essential. I also keep a close eye on their nutrition and hydration, especially during intense training.");
                            healthModule.AddOption("What common health issues do they face?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Snow dogs can face issues like hip dysplasia and frostbite if not properly cared for. That's why constant attention is necessary.")));
                                });
                            healthModule.AddOption("How do you exercise them?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I take them on long runs across the tundra, allowing them to explore and enjoy their natural instincts. It's great for their physical and mental health.")));
                                });
                            pl.SendGump(new DialogueGump(pl, healthModule));
                        });
                    raisingModule.AddOption("What challenges do you face?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule challengesModule = new DialogueModule("The harsh weather can be unforgiving, and sometimes food can be scarce. I also face challenges from wildlife, but I always remain vigilant.");
                            challengesModule.AddOption("How do you protect them from wildlife?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I keep a close watch during their playtime and training sessions. If necessary, I use barriers and enclosures to keep them safe.")));
                                });
                            challengesModule.AddOption("What about when it's too cold?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("On extremely cold days, I limit their time outside and make sure they stay warm inside their shelters.")));
                                });
                            pl.SendGump(new DialogueGump(pl, challengesModule));
                        });
                    player.SendGump(new DialogueGump(player, raisingModule));
                });

            greeting.AddOption("Do you sell snow dogs?",
                player => true,
                player => 
                {
                    DialogueModule sellModule = new DialogueModule("I do not sell my snow dogs, but I am always willing to share knowledge about raising them. They are my family, after all.");
                    sellModule.AddOption("Can you help me train my dog?",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I would love to help you train your dog! Training is a rewarding experience that strengthens your bond.")));
                        });
                    sellModule.AddOption("Can you recommend a good breeder?",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I suggest looking for reputable breeders who prioritize health and temperament. The Arctic is home to many fine breeders.")));
                        });
                    player.SendGump(new DialogueGump(player, sellModule));
                });

            greeting.AddOption("What do you do for fun?",
                player => true,
                player =>
                {
                    DialogueModule funModule = new DialogueModule("In my free time, I enjoy taking my dogs out for long hikes, exploring the beautiful Arctic landscape. I also like to share stories with fellow dog enthusiasts around the campfire.");
                    funModule.AddOption("What stories do you share?",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I love to tell tales of my adventures with my snow dogs—how they helped me navigate through blizzards and their antics during playtime.")));
                        });
                    funModule.AddOption("Do you ever participate in dog sled races?",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Yes! Racing with my dogs is thrilling. They love to run, and seeing them in their element brings me immense joy.")));
                        });
                    player.SendGump(new DialogueGump(player, funModule));
                });

            greeting.AddOption("Why do you love snow dogs?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Snow dogs embody strength, loyalty, and spirit. Their companionship enriches my life, and their joy is infectious. I can't imagine my life without them.")));
                });

            return greeting;
        }

        private bool CanReward()
        {
            return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
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
