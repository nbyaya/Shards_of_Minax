using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the venomous overlord")]
    public class VenomousAssassinBoss : VenomousAssassin
    {
        [Constructable]
        public VenomousAssassinBoss() : base()
        {
            Name = "Venomous Overlord";
            Title = "the Supreme Assassin";

            // Update stats to match or exceed Barracoon's (or original values)
            SetStr(700); // Boost strength to be more formidable
            SetDex(300); // Higher dexterity for faster attacks
            SetInt(150); // Keep intelligence the same, can be adjusted if needed

            SetHits(12000); // Enhanced health to make it more challenging
            SetDamage(25, 40); // Increased damage output

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Boost resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Enhance skills to make the boss more challenging
            SetSkill(SkillName.Anatomy, 90.1, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Ninjitsu, 120.0);
            SetSkill(SkillName.Hiding, 100.0);
            SetSkill(SkillName.Stealth, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach a random ability for dynamic gameplay
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
            // Additional boss logic, such as more frequent speeches or special actions
        }

        public VenomousAssassinBoss(Serial serial) : base(serial)
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
