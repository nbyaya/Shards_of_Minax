using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class RelativityManual : Item
    {
        [Constructable]
        public RelativityManual() : base(0xFEF)
        {
            Name = "Relativity Manual";
            Hue = 0x489; // A bright blue color
            Weight = 1.0;
        }

        public RelativityManual(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("You feel your intelligence expanding as you read the Relativity Manual.");
            from.RawInt += 5;
            from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
            from.PlaySound(0x1EA);
            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GeniusGlasses : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 5; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 5; } }
        public override int BaseEnergyResistance { get { return 5; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 60; } }

        public override int AosStrReq { get { return 45; } }
        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 30; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public GeniusGlasses() : base(0x2FB8)
        {
            Name = "Genius Glasses";
            Hue = 0x47E; // A golden color
            Weight = 2.0;
        }

        public GeniusGlasses(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;
                from.SendMessage("You feel your magical abilities improving as you wear the Genius Glasses.");
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;
                from.SendMessage("You feel your enhanced magical abilities fading as you remove the Genius Glasses.");
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Improves Spell Damage: 15%");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}