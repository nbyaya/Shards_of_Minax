using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master con artist")]
    public class ConArtistBoss : ConArtist
    {
        [Constructable]
        public ConArtistBoss() : base()
        {
            Name = "Master Con Artist";
            Title = "the Grand Deceiver";

            // Update stats to match or exceed Barracoon's (or better)
            SetStr(700); // Enhanced strength
            SetDex(200); // Enhanced dexterity
            SetInt(400); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(20, 35); // Enhanced damage range

            // Boost resistance values to make the boss more formidable
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Magery, 90.0, 120.0); // Higher magery skill
            SetSkill(SkillName.MagicResist, 85.0, 110.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 90.0, 110.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 90.0, 110.0); // Enhanced wrestling skill

            Fame = 22500; // Increase fame to match boss-tier status
            Karma = -22500; // Maintain the negative karma of the original

            VirtualArmor = 60; // Enhanced armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new AbysmalHorrorSummoningMateria());
			PackItem(new HindSummoningMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public ConArtistBoss(Serial serial) : base(serial)
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
