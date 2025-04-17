using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a venom sprite corpse")]
    public class VenomSprite : BaseCreature
    {
        private Mobile m_Caster;

        [Constructable]
        public VenomSprite()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Venom Sprite";
            Body = 128;
            Hue = 0x48E; // Toxic green
            BaseSoundID = 0x467;


            double skillFactor = 0;

            SetStr((int)(30 * skillFactor), (int)(40 * skillFactor));
            SetDex((int)(300 * skillFactor), (int)(400 * skillFactor));
            SetInt((int)(250 * skillFactor), (int)(300 * skillFactor));

            SetHits((int)(60 * skillFactor), (int)(80 * skillFactor));
            SetDamage((int)(10 * skillFactor), (int)(20 * skillFactor));

            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 90, 100); // Nearly immune
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);
            SetSkill(SkillName.Poisoning, 100.0); // High poison chance

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            // Optional: glow or aura effect
            AddItem(new LightSource()); 
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.3)
            {
                attacker.ApplyPoison(this, Poison.Deadly);
                attacker.SendMessage("The sprite’s toxic aura seeps into your flesh!");
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.5)
            {
                defender.ApplyPoison(this, Poison.Lethal);
                defender.SendMessage("Venom burns through your veins!");
            }
        }

        public override void GenerateLoot()
        {
            // No loot since it's a summon
        }

        public override bool DeleteCorpseOnDeath => true;
        public override bool BardImmune => true;
        public override bool Unprovokable => true;
        public override bool AlwaysMurderer => true;
        public override bool Commandable => false;

        public VenomSprite(Serial serial) : base(serial)
        {
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
