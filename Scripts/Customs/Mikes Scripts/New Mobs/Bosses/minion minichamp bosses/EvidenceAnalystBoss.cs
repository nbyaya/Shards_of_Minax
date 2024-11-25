using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the evidence overlord")]
    public class EvidenceAnalystBoss : EvidenceAnalyst
    {
        [Constructable]
        public EvidenceAnalystBoss() : base()
        {
            Name = "Evidence Overlord";
            Title = "the Supreme Analyst";

            // Update stats to match or exceed Barracoon's (or better)
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(150); // Matching Barracoon's upper dexterity
            SetInt(750); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Matching Barracoon's upper stamina
            SetMana(750); // Matching Barracoon's upper mana

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 70.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

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
			PackItem(new FlaskCircleMateria());
			PackItem(new HeadBreathMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Speak some final words
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My analysis... is final..."); break;
                case 1: this.Say(true, "You will regret crossing me..."); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if desired
        }

        public EvidenceAnalystBoss(Serial serial) : base(serial)
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
