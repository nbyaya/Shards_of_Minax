using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Irene of Athens")]
    public class IreneOfAthens : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public IreneOfAthens() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Irene of Athens";
            Body = 0x191; // Human female body

            // Stats
            SetStr(100);
            SetDex(60);
            SetInt(80);
            SetHits(70);

            // Appearance
            AddItem(new FancyDress() { Hue = 0xB3D9 }); // Snow White Hue
            AddItem(new Boots() { Hue = 0xB3D9 }); // Snow White Hue
            AddItem(new GoldRing() { Name = "Irene of Athens's Ring" });

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

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Irene of Athens, and my days in this wretched city are filled with misery and contempt. What would you like to discuss?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My health is as poor as the state of this wretched empire. I often wonder if there's hope for recovery.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player => 
                {
                    DialogueModule jobModule = new DialogueModule("My \"job\" in this forsaken land? I am but a pawn in the games of power, nothing more. Sometimes, I wish I could escape this cycle.");
                    jobModule.AddOption("Is there a way to escape?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Escaping the clutches of this empire seems impossible. Yet, perhaps a brave adventurer could change the fate of many.")));
                        });
                    jobModule.AddOption("Do you regret your choices?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Regret is a bitter companion. I sometimes wish I had chosen a different path, one filled with light rather than shadows.")));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Tell me about battles.",
                player => true,
                player => 
                {
                    DialogueModule battleModule = new DialogueModule("True valor is but a distant memory in this city. Are you any different? Many brave souls have fallen.");
                    battleModule.AddOption("What do you mean by that?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("I mean that too often, the brave are silenced by greed and corruption. Do you not see it around you?")));
                        });
                    battleModule.AddOption("I strive to be different.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Then you are a rare gem in this rough landscape. But beware; the path of valor is fraught with peril.")));
                        });
                    player.SendGump(new DialogueGump(player, battleModule));
                });

            greeting.AddOption("I want to hear about your past.",
                player => true,
                player => 
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        player.SendGump(new DialogueGump(player, new DialogueModule("In my youth, I was filled with dreams and aspirations, but the harsh realities of life in this city have withered them away. Here, take this small token from those happier times.")));
                    }
                });

            greeting.AddOption("What do you think about glory?",
                player => true,
                player => 
                {
                    DialogueModule gloryModule = new DialogueModule("The days of glory, when honor and valor ruled, seem like distant memories now. Glory fades, but its memory lingers.");
                    gloryModule.AddOption("Is there hope for glory again?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Hope is a fragile thing. It requires belief and courage. Perhaps, if enough brave souls stand together, glory can be reclaimed.")));
                        });
                    player.SendGump(new DialogueGump(player, gloryModule));
                });

            greeting.AddOption("What about the mighty?",
                player => true,
                player => 
                {
                    DialogueModule mightyModule = new DialogueModule("Those who consider themselves mighty often forget the simple truths of life. Their power fades with time, leaving only shadows.");
                    mightyModule.AddOption("What do you mean by shadows?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Shadows represent the remnants of their once-great power. Without wisdom and humility, even the mighty will fall into obscurity.")));
                        });
                    player.SendGump(new DialogueGump(player, mightyModule));
                });

            return greeting;
        }

        public IreneOfAthens(Serial serial) : base(serial) { }

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
