using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the cowgirl cyclops overlord")]
    public class CountryCowgirlCyclopsBoss : CountryCowgirlCyclops
    {
        [Constructable]
        public CountryCowgirlCyclopsBoss() : base()
        {
            Name = "Cyclops Cowgirl Overlord";
            Title = "the Lassoing Tyrant";

            // Enhance stats to match or exceed Barracoon's stats
            SetStr(1200); // Matching or exceeding Barracoon's upper strength
            SetDex(255);  // Matching or exceeding Barracoon's upper dexterity
            SetInt(250);  // Matching or exceeding Barracoon's upper intelligence

            SetHits(12000); // Significantly increased health
            SetStam(300); // Enhanced stamina
            SetMana(750); // Enhanced mana

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 22500; // Increased fame to match a boss-tier NPC
            Karma = -22500; // Same for karma

            VirtualArmor = 80; // Enhanced armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here (if needed)
        }

        public CountryCowgirlCyclopsBoss(Serial serial) : base(serial)
        {
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
