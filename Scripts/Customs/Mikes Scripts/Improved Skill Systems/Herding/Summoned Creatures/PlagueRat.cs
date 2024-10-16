using System;

namespace Server.Mobiles
{
    [CorpseName("a filth rat corpse")]
    public class FilthRat : BaseCreature
    {
        private DateTime m_NextDiseaseSpread;

        [Constructable]
        public FilthRat()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a filth rat";
            this.Body = 238;
            this.BaseSoundID = 0xCC;
            this.Hue = 1425; // Unique hue for the plague rat

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextDiseaseSpread = DateTime.UtcNow;
        }

        public FilthRat(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextDiseaseSpread)
            {
                SpreadDisease();
            }
        }

        private void SpreadDisease()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                // Inflict disease
                target.ApplyPoison(this, Poison.Deadly);
                target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                target.PlaySound(0x205);

                // Spread disease to nearby enemies
                foreach (Mobile m in this.GetMobilesInRange(2))
                {
                    if (m != this && m != target && m.Alive && !m.IsDeadBondedPet && this.CanBeHarmful(m))
                    {
                        m.ApplyPoison(this, Poison.Regular);
                        m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                        m.PlaySound(0x205);
                    }
                }

                m_NextDiseaseSpread = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }
		
		public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
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
