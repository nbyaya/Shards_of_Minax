using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the combat medic")]
    public class CombatNurseBoss : CombatNurse
    {
        [Constructable]
        public CombatNurseBoss() : base()
        {
            Name = "Combat Medic";
            Title = "the Healing Overlord";

            // Enhanced stats
            SetStr(800); // Stronger strength than the original
            SetDex(200); // Higher dexterity
            SetInt(400); // Higher intelligence

            SetHits(12000); // Much higher health for a boss-tier NPC

            SetDamage(15, 30); // Higher damage output

            SetResistance(ResistanceType.Physical, 70, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 120.0); // High skill for healing and tactics
            SetSkill(SkillName.Healing, 120.0); // Increased healing skill
            SetSkill(SkillName.Magery, 120.0); // Higher magic resistance
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500; // Higher fame for a boss
            Karma = 22500; // High karma (or adjust as per the type of boss)

            VirtualArmor = 75; // Increased armor value

            // Attach the XmlRandomAbility for dynamic special abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new FrostwardensPlateChest());
			PackItem(new ArcticOgreLordSummoningMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional logic for the boss (you can add further special abilities here)
        }

        public CombatNurseBoss(Serial serial) : base(serial)
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
