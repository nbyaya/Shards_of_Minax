using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Guardian Basil")]
    public class GuardianBasil : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GuardianBasil() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Guardian Basil";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(90);
            SetInt(60);
            SetHits(90);

            // Appearance
            AddItem(new ChainChest() { Hue = 1953 });
            AddItem(new ChainLegs() { Hue = 1953 });
            AddItem(new ChainCoif() { Hue = 1953 });
            AddItem(new LeatherGloves() { Hue = 1953 });
            AddItem(new Boots() { Hue = 1953 });
            AddItem(new Halberd() { Name = "Basil's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue;
        }

        public GuardianBasil(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Guardian Basil, the keeper of ancient secrets. What do you seek?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("My essence remains untouched."))));

            greeting.AddOption("What is your job?",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("I guard the gateway to hidden realms. This task is both a privilege and a burden."))));

            greeting.AddOption("What secrets do you guard?",
                player => true,
                player =>
                {
                    DialogueModule secretsModule = new DialogueModule("The secrets I protect are ancient and powerful. Do you seek power or wisdom?");
                    secretsModule.AddOption("I seek wisdom.",
                        p => true,
                        p =>
                        {
                            TimeSpan cooldown = TimeSpan.FromMinutes(10);
                            if (DateTime.UtcNow - lastRewardTime < cooldown)
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("I have no reward right now. Please return later.")));
                            }
                            else
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Wisdom is more valuable than any treasure. Here, take this token of knowledge.")));
                                p.AddToBackpack(new MaxxiaScroll());
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            }
                        });
                    secretsModule.AddOption("I seek power.",
                        p => true,
                        p =>
                        {
                            DialogueModule powerModule = new DialogueModule("Power can be a double-edged sword. It can grant you the ability to protect or to destroy. Do you understand the weight of this responsibility?");
                            powerModule.AddOption("Yes, I understand.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Then wield it wisely, for power is often corrupted."))));
                            powerModule.AddOption("No, tell me more.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("To seek power without understanding can lead to ruin. Be cautious on your journey."))));
                            p.SendGump(new DialogueGump(p, powerModule));
                        });
                    player.SendGump(new DialogueGump(player, secretsModule));
                });

            greeting.AddOption("Tell me about the ancients.",
                player => true,
                player =>
                {
                    DialogueModule ancientsModule = new DialogueModule("The ancients were beings of immense knowledge and magic. Their legacy is scattered throughout the land.");
                    ancientsModule.AddOption("Where can I find their knowledge?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Many ancient texts are hidden in forgotten ruins or guarded by powerful creatures. Seek them out, and you may uncover the truth.")));
                        });
                    ancientsModule.AddOption("What did they accomplish?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("They crafted powerful artifacts and unlocked the secrets of life and death. Their influence lingers still, waiting to be discovered.")));
                        });
                    player.SendGump(new DialogueGump(player, ancientsModule));
                });

            greeting.AddOption("How do I prove my worth?",
                player => true,
                player =>
                {
                    DialogueModule worthModule = new DialogueModule("To prove your worth, you must face challenges and tests. But fear not, for I sense a strong spirit within you. Are you ready to embark on this quest?");
                    worthModule.AddOption("Yes, I am ready.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well! Your journey begins now. Seek out the trials hidden in the Whispering Forest, where your courage will be tested.")));
                        });
                    worthModule.AddOption("No, tell me more first.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The trials are not for the faint of heart. They will test your resolve, intellect, and strength. Only those who succeed may receive the knowledge of the ancients.")));
                        });
                    player.SendGump(new DialogueGump(player, worthModule));
                });

            greeting.AddOption("What do you know about the realms?",
                player => true,
                player =>
                {
                    DialogueModule realmsModule = new DialogueModule("The realms are vast and filled with wonders and dangers. Each realm has its own unique secrets and challenges.");
                    realmsModule.AddOption("What realms exist?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("There are realms of elemental power, mystical creatures, and lost civilizations. Each holds a piece of the puzzle that is our existence."))));
                    realmsModule.AddOption("How do I access these realms?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("To access these realms, you must first gain the knowledge or power required to open the gateways. Often, this involves completing certain tasks or challenges."))));
                    player.SendGump(new DialogueGump(player, realmsModule));
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
