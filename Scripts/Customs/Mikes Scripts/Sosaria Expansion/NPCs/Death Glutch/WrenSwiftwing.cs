using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SkyfiresWrathQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Skyfire’s Wrath"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Wren Swiftwing*, the seasoned messenger of Death Glutch, her leather satchel scarred by claws, her eyes sharp as a hawk’s.\n\n" +
                    "She stands beside a battered post, parchments scattered, smoke rising faintly from a burnt dispatch tube.\n\n" +
                    "“That beast’s not just a menace—it’s a message. The Mystic Dragon's grounding our flights, turning skyways into deathtraps.”\n\n" +
                    "“I clipped a scale from its wing—*bare-handed*. It screamed. It remembers me. And now, I want its heart.”\n\n" +
                    "“Each missive undelivered brings us closer to war. I need someone fast, ruthless, and relentless.”\n\n" +
                    "**Slay the Mystic Dragon** in the depths of *Malidor Witches Academy*. Reclaim the skies—for all of us.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Fine. Stay grounded. But know this—when war comes, silence will be our assassin.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it soars? The couriers fall, one by one. Even the bravest wings can't dodge flame forever.";
            }
        }

        public override object Complete
        {
            get
            {
                return "*Wren grips the charred scale from the dragon’s wing, her eyes narrowing.*\n\n" +
                       "“It’s done, then? The sky’s ours again.”\n\n" +
                       "**She hands you a hood, woven in shadows.**\n\n" +
                       "“Wear it. Let them know you fly for Death Glutch now—and that we don't forget who kept us soaring.”";
            }
        }

        public SkyfiresWrathQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MysticDragon), "Mystic Dragon", 1));
            AddReward(new BaseReward(typeof(CovensShadowedHood), 1, "CovensShadowedHood"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Skyfire’s Wrath'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WrenSwiftwing : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SkyfiresWrathQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public WrenSwiftwing()
            : base("the Sky Courier", "Wren Swiftwing")
        {
        }

        public WrenSwiftwing(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 95, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Weathered tan
            HairItemID = 0x2048; // Long hair
            HairHue = 1157; // Midnight black
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherNinjaJacket() { Hue = 1109, Name = "Stormrunner's Jacket" }); // Charcoal black
            AddItem(new LeatherShorts() { Hue = 1154, Name = "Skyraider's Breeches" }); // Storm-blue
            AddItem(new NinjaTabi() { Hue = 1109, Name = "Whisperstep Boots" }); // Charcoal black
            AddItem(new LeatherNinjaHood() { Hue = 1154, Name = "Wingwatch Hood" }); // Storm-blue
            AddItem(new BodySash() { Hue = 1175, Name = "Courier’s Mark" }); // Sky silver

            AddItem(new RepeatingCrossbow() { Hue = 2418, Name = "Swiftwind Repeater" }); // Ember red bowstring

            Backpack backpack = new Backpack();
            backpack.Hue = 2101; // Faded leather
            backpack.Name = "Sky Courier's Satchel";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
