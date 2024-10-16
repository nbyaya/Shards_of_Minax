using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyric Lila")]
    public class LyricLila : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyricLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyric Lila";
            Body = 0x191; // Human female body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Stats
            SetStr(118);
            SetDex(68);
            SetInt(84);
            SetHits(87);

            // Appearance
            AddItem(new FancyDress() { Hue = 54 });
            AddItem(new Shoes() { Hue = 66 });
            AddItem(new LeatherGloves() { Name = "Lila's Lyric Gloves" });

            // Initialize lastRewardTime
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Lyric Lila, the bard.");

            greeting.AddOption("What do you do?",
                player => true,
                player => {
                    DialogueModule jobModule = new DialogueModule("I weave tales and sing songs that touch the soul. I am a bard.");
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("How are you?",
                player => true,
                player => {
                    DialogueModule healthModule = new DialogueModule("I'm in good spirits, thank you for asking.");
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("Tell me about the virtues.",
                player => true,
                player => {
                    DialogueModule virtuesModule = new DialogueModule("Ah, the eight virtues, they are the essence of a noble heart. Which virtue doth thou seek insight into?");
                    AddVirtueOptions(virtuesModule, player);
                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("What can you tell me about music?",
                player => true,
                player => {
                    DialogueModule musicModule = new DialogueModule("Music is the universal language. It transcends borders and speaks to the soul. For your genuine interest, let me give you this special lute as a gift.");
                    if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                    {
                        musicModule.NPCText += " Here, it's yours.";
                        player.AddToBackpack(new SpellweavingAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, musicModule));
                });

            greeting.AddOption("Tell me about your ancestors.",
                player => true,
                player => {
                    DialogueModule ancestorsModule = new DialogueModule("My ancestors were renowned bards, traveling the world and gathering tales. Their songs are still remembered and cherished.");
                    player.SendGump(new DialogueGump(player, ancestorsModule));
                });

            return greeting;
        }

        private void AddVirtueOptions(DialogueModule virtuesModule, PlayerMobile player)
        {
            virtuesModule.AddOption("Tell me about honesty.",
                pl => true,
                pl => {
                    DialogueModule honestyModule = new DialogueModule("Honesty, the foundation of trust. To be honest is to be true to oneself and others.");
                    pl.SendGump(new DialogueGump(pl, honestyModule));
                });

            virtuesModule.AddOption("What about courage?",
                pl => true,
                pl => {
                    DialogueModule courageModule = new DialogueModule("Courage is not the absence of fear, but the resolve to face it. A true hero embraces their fears and acts regardless.");
                    pl.SendGump(new DialogueGump(pl, courageModule));
                });
            
            // Add more virtues as needed
        }

        public LyricLila(Serial serial) : base(serial) { }

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
