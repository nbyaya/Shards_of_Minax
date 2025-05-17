using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a glacial overlord corpse")]
    public class IcyFrostTroll : BaseCreature
    {
        private long _NextAura;
        private DateTime m_Delay;
        private DateTime m_GlacialCooldown;

        [Constructable]
        public IcyFrostTroll() : base(AIType.AI_Mage, FightMode.Closest, 25, 1, 0.3, 0.5)
        {
            Name = "Glacial Overlord";
            Body = 55;
            BaseSoundID = 461;
            Hue = 1152; // Bright ice blue

            SetStr(350, 400);
            SetDex(100, 125);
            SetInt(150, 200);

            SetHits(800, 900);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 70;
            Tamable = false;

            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
            _NextAura = Core.TickCount + 3000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 4);
            AddLoot(LootPack.LowScrolls, 2);
            AddLoot(LootPack.Gems, 10);
            
            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new FrostTrollCrown());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            // Frost Aura every 5 seconds
            if (Core.TickCount >= _NextAura)
            {
                DoFrostAura();
                _NextAura = Core.TickCount + 5000;
            }

            // Special Abilities
            if (DateTime.UtcNow > m_Delay)
            {
                if (0.5 > Utility.RandomDouble())
                {
                    if (Combatant is Mobile target)
                        BlizzardBreath(target);
                }
                else
                {
                    AvalancheCall();
                }

                m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }

            // Glacial Smash cooldown
            if (DateTime.UtcNow > m_GlacialCooldown)
            {
                if (Combatant is Mobile target && InRange(target, 2))
                {
                    GlacialSmash(target);
                    m_GlacialCooldown = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }
            }
        }

        #region Unique Abilities
        private void DoFrostAura()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                {
                    m.SendMessage("You're frozen by the glacial aura!");
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 100, 0, 0);
                    m.FixedEffect(0x374A, 10, 30);
                    m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                }
            }
        }

        public void BlizzardBreath(Mobile target)
        {
            this.Direction = this.GetDirectionTo(target);
            this.Animate(12, 5, 1, true, false, 0);

            target.FixedParticles(0x376A, 1, 30, 9965, 1152, 0, EffectLayer.Waist);
            target.PlaySound(0x1F9);
            AOS.Damage(target, this, Utility.RandomMinMax(60, 80), 0, 0, 100, 0, 0);
            
            if (target is PlayerMobile && 0.5 > Utility.RandomDouble())
            {
                target.Paralyze(TimeSpan.FromSeconds(3));
                target.SendMessage("The freezing breath paralyzes you!");
            }
        }

        private void AvalancheCall()
        {
            this.Say(true, "Feel the mountain's wrath!");
            new AvalancheTimer(this, this.Location).Start();
        }

        private void GlacialSmash(Mobile target)
        {
            this.Say(true, "Crush beneath the glacier!");
            this.Animate(20, 5, 1, true, false, 0);
            
            AOS.Damage(target, this, Utility.RandomMinMax(80, 100), 0, 0, 100, 0, 0);
            target.FixedEffect(0x37B9, 10, 30);
            target.SendMessage("The glacial smash freezes your veins!");
            
            if (target is PlayerMobile)
                target.ApplyPoison(this, Poison.Greater);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);
            
            if (0.3 > Utility.RandomDouble())
                SummonIceWisp(caster);
        }

        private void SummonIceWisp(Mobile target)
        {
            IceWisp wisp = new IceWisp();
            wisp.MoveToWorld(this.Location, this.Map);
            wisp.Combatant = target;
            wisp.Say("The glacier defends its master!");
        }
        #endregion

        #region Timer Classes
		private class AvalancheTimer : Timer
		{
			private readonly Mobile m_Troll;
			private readonly Point3D m_Location;
			private int m_Count;

			public AvalancheTimer(Mobile troll, Point3D loc) 
				: base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
			{
				m_Troll = troll;
				m_Location = loc;
				m_Count = 0;
			}

			protected override void OnTick()
			{
				if (m_Count++ >= 6 || m_Troll.Deleted)
				{
					Stop();
					return;
				}

				Map map = m_Troll.Map;
				if (map == null)
					return;

				for (int i = -2; i <= 2; i++)
				{
					Point3D p = new Point3D(m_Location.X + i, m_Location.Y + m_Count, m_Location.Z);

					Effects.SendLocationEffect(p, map, 0x36B0, 20, 10, 1152, 0);
					Effects.PlaySound(p, map, 0x307);

					foreach (Mobile m in map.GetMobilesInRange(p, 0))
					{
						if (m != m_Troll && m_Troll.CanBeHarmful(m))
						{
							AOS.Damage(m, m_Troll, Utility.RandomMinMax(40, 60), 0, 0, 100, 0, 0);
							m.SendMessage("You're struck by falling ice!");
						}
					}
				}
			}
		}

        #endregion

        #region Loot and Properties
        public override int Meat => 10;
        public override int Hides => 20;
        public override HideType HideType => HideType.Spined;
        public override bool BardImmune => true;
        public override bool Unprovokable => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public IcyFrostTroll(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
        #endregion
    }

    public class IceWisp : BaseCreature
    {
        [Constructable]
        public IceWisp() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ice wisp";
            Body = 58;
            Hue = 1152;
            BaseSoundID = 466;

            SetStr(100);
            SetDex(150);
            SetInt(100);

            SetHits(80);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Magery, 60.0);
            SetSkill(SkillName.EvalInt, 60.0);
            SetSkill(SkillName.MagicResist, 60.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 40;
        }

        public IceWisp(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}