using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells; 

namespace Server.Mobiles
{
    [CorpseName("a hemoglint stryga corpse")]
    public class HemoglintStryga : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextBloodMist;
        private DateTime m_NextRiftTime;
        private DateTime m_NextChainTime;
        private Point3D m_LastLocation;

        // Unique Hue: a deep, blood-red tint
        private const int UniqueHue = 1175;

        [Constructable]
        public HemoglintStryga()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Hemoglint Stryga";
            Body = 317;            // Vampire bat body
            BaseSoundID = 0x270;   // Vampire bat sounds
            Hue = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(400, 450);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 40);
            SetDamageType(ResistanceType.Energy, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 100.1, 110.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 110.1, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 85.1, 95.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 75;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextBloodMist = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextChainTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new BatWing(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
        }

        // --- Aura: Blood Drain on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 3) && this.Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Stamina Drain
                    int stamDrain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x22, "The Hemoglint Stryga's presence saps your strength!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }

                    // Minor Poisony Strike
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 40, 0, 60);

                    // Chance to apply deadly poison
                    if (Utility.RandomDouble() < 0.2)
                    {
                        target.ApplyPoison(this, Poison.Deadly);
                        target.SendMessage("You have been poisoned by the stryga's bite!");
                    }
                }
            }
        }

        // --- Main AI Loop for Special Attacks & Trail Hazards ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a toxic blood pool behind occasionally
            if (this.Location != m_LastLocation && this.Map != null && Utility.RandomDouble() < 0.20)
            {
                Point3D old = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    PoisonTile pit = new PoisonTile();
                    pit.Hue = UniqueHue;
                    pit.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // In-range checks for abilities
            if (DateTime.UtcNow >= m_NextBloodMist && InRange(Combatant.Location, 8))
            {
                BloodMistAttack();
                m_NextBloodMist = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && InRange(Combatant.Location, 12))
            {
                SanguineRiftAttack();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 26));
            }
            else if (DateTime.UtcNow >= m_NextChainTime && InRange(Combatant.Location, 10))
            {
                VampiricChainAttack();
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 36));
            }
        }

        // --- Ability: Blood Mist (AoE Poison + Drain) ---
		public void BloodMistAttack()
		{
			this.Say("*Feel the veil of death!*");
			PlaySound(0x212);
			Effects.SendLocationParticles(
				EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
				0x3779, 12, 20, UniqueHue, 0, 5044, 0);

			var targets = new List<Mobile>(); // 💡 Declare targets here
			var eable = Map.GetMobilesInRange(this.Location, 6);
			foreach (Mobile m in eable)
			{
				if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
					targets.Add(m);
			}
			eable.Free();

			foreach (var t in targets)
			{
				DoHarmful(t);

				// Poison damage
				AOS.Damage(t, this, Utility.RandomMinMax(30, 50), 0, 0, 40, 0, 60);

				if (t is Mobile mob)
				{
					// Stamina drain
					int sDrain = Utility.RandomMinMax(10, 20);
					if (mob.Stam >= sDrain)
					{
						mob.Stam -= sDrain;
						mob.SendMessage(0x22, "You are weakened by the sanguine mist!");
						mob.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
						mob.PlaySound(0x1F8);
					}

					// Mana drain
					int mDrain = Utility.RandomMinMax(5, 15);
					if (mob.Mana >= mDrain)
					{
						mob.Mana -= mDrain;
						mob.SendMessage(0x21, "Your life essence is consumed!");
						mob.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
						mob.PlaySound(0x1F9);
					}
				}
			}
		}


        // --- Ability: Sanguine Rift (Targeted Hazard) ---
        public void SanguineRiftAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Blood will bind you!*");
            PlaySound(0x22F);

            Point3D loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return;

                Point3D spawn = loc;
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                {
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);
                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                        return;
                }

                var tile = new NecromanticFlamestrikeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawn, this.Map);

                Effects.PlaySound(spawn, this.Map, 0x1F6);
            });
        }

        // --- Ability: Vampiric Chain (Bouncing Life Drain) ---
        public void VampiricChainAttack()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false) || !SpellHelper.ValidIndirectTarget(this, initial))
                return;

            this.Say("*Succumb to my hunger!*");
            PlaySound(0x20A);

            var targets = new List<Mobile> { initial };
            int maxBounces = 4;
            double maxRange = 5.0;

            for (int i = 0; i < maxBounces; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double closest = double.MaxValue;

				var eable = Map.GetMobilesInRange(last.Location, (int)maxRange);
				foreach (Mobile m in eable)
				{
					if (m != this && m != last && !targets.Contains(m)
						&& CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m)
						&& last.InLOS(m))
					{
						double d = last.GetDistanceToSqrt(m);
						if (d < closest)
						{
							closest = d;
							next = m;
						}
					}
				}
				eable.Free(); // ✅ Manually release


                if (next == null) break;
                targets.Add(next);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        int dmg = Utility.RandomMinMax(25, 40);
                        AOS.Damage(dst, this, dmg, 0, 0, 0, 0, 100);

                        // Heal half the damage dealt
                        int heal = dmg / 2;
                        this.Hits = Math.Min(this.Hits + heal, this.HitsMax);

                        dst.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Explosion: Blood & Flame Hazards ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My essence… remains…*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                int count = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < count; i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3);
                    int yOff = Utility.RandomMinMax(-3, 3);
                    var spawn = new Point3D(this.X + xOff, this.Y + yOff, this.Z);

                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    {
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);
                        if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                            continue;
                    }

                    Item tile;
                    if (Utility.RandomBool())
                        tile = new PoisonTile();
                    else
                        tile = new NecromanticFlamestrikeTile();

                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(spawn, this.Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(spawn, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Properties ---
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new BatWing(Utility.RandomMinMax(5, 10)));  // Rare batch

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll()); // Unique artifact placeholder
        }

        // --- Serialization ---
        public HemoglintStryga(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version

            // Re-init cooldowns
            m_NextBloodMist = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRiftTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextChainTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation = this.Location;
        }
    }
}
