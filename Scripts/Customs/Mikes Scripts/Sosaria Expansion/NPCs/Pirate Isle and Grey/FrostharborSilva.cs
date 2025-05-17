using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FenrirsFreezeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fenrir’s Freeze"; } }

        public override object Description
        {
            get
            {
                return
                    "*Frostharbor Silva*, wrapped in furs, her breath misting in the salty air, fixes you with an icy glare.\n\n" +
                    "“They call me mad, fishin’ ice in warm waters. But there’s truth in the cold, aye? Truth... and monsters.”\n\n" +
                    "“A beast, **FrozenFenrir**, it’s ruined me lures. Cracked ‘em clean under its breath—ice sharp as any blade. And its howl... still feel it in me bones.”\n\n" +
                    "“The thing stalks the flooded tunnels near *Exodus*. Can’t haul a line with that curse breathing down me neck.”\n\n" +
                    "**Slay the FrozenFenrir**, and maybe I’ll fish in peace again. Maybe.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then you’d best not linger near the docks. That howl’s in the mist, and soon in your soul if you’re not careful.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathin’, is it? The chill’s spread. Cracked more than lures—it’s crackin’ the Isle itself.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve silenced the beast? Bless the cold stars...\n\n" +
                       "Take this: *MagesRelicChest*. Found it ‘neath the ice once, before that Fenrir cursed the tunnels. Maybe it’ll serve you better than it ever did me.";
            }
        }

        public FenrirsFreezeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrozenFenrir), "FrozenFenrir", 1));
            AddReward(new BaseReward(typeof(MagesRelicChest), 1, "MagesRelicChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fenrir’s Freeze'!");
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

    public class FrostharborSilva : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FenrirsFreezeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman());
        }

        [Constructable]
        public FrostharborSilva()
            : base("the Ice-Fisher", "Frostharbor Silva")
        {
        }

        public FrostharborSilva(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale frost-touched skin
            HairItemID = 0x2046; // Long Hair
            HairHue = 1150; // Icy White
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1153, Name = "Frosthide Sarong" });
            AddItem(new FurBoots() { Hue = 1150, Name = "Seafarer's Boots" });
            AddItem(new Cloak() { Hue = 1153, Name = "Icebound Cloak" });
            AddItem(new BearMask() { Hue = 1152, Name = "Snowfang Mask" });
            AddItem(new HalfApron() { Hue = 1150, Name = "Fishscale Apron" });
            AddItem(new Pitchfork() { Hue = 1154, Name = "Ice-Fisher's Trident" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Salt-Weathered Pack";
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
