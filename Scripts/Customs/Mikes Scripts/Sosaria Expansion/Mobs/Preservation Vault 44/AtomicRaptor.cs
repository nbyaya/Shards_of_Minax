using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an Atomic Raptor carcass")]
    public class AtomicRaptor : BaseCreature
    {
        // Ability cooldown trackers
        private DateTime m_NextBlastTime;
        private DateTime m_NextMutationTime;
        private DateTime m_NextSpawnTime;
        private Point3D m_LastLocation;

        // Unique radioactive green
        private const int UniqueHue = 2405;

        [Constructable]
        public AtomicRaptor() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Atomic Raptor";
            Body = 730;
            Hue = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(150, 180);
            SetInt(100, 120);

            SetHits(600, 700);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(15, 25);
            
            // Damage focus: mostly physical with some energy
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Energy, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics,     95.0, 105.0);
            SetSkill(SkillName.Wrestling,   90.0, 100.0);

            Fame = 12500;
            Karma = -12500;

            VirtualArmor = 50;
            ControlSlots = 4;
            Tamable = false;

            // Initialize ability timers
            var now = DateTime.UtcNow;
            m_NextBlastTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextMutationTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSpawnTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation     = this.Location;
        }

        // Radiological trail: leaves ToxicGasTile behind
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Deleted || this.Map == null) return;
            // Only drop occasionally when we actually move
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.30)
            {
                var loc = m_LastLocation;
                int z = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new ToxicGasTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
            }
            m_LastLocation = this.Location;
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Nuclear Blast: AoE around self
            if (now >= m_NextBlastTime)
            {
                NuclearBlast();
                m_NextBlastTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Mutagenic Bite: single‐target debuff + poison
            else if (now >= m_NextMutationTime && InRange(Combatant.Location, 4))
            {
                MutagenicBite();
                m_NextMutationTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            // Spawn Mutated Mini‑Raptors
            else if (now >= m_NextSpawnTime)
            {
                SpawnMinions();
                m_NextSpawnTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        private void NuclearBlast()
        {
            this.Say("*The raptor roars and releases a nuclear shockwave!*");
            PlaySound(0x208); // explosion sound
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3779, 20, 30, UniqueHue, 0, 5032, 0);

            var targets = new List<Mobile>();
            foreach (var obj in Map.GetMobilesInRange(Location, 6))
            {
                if (obj != this && CanBeHarmful(obj, false) && SpellHelper.ValidIndirectTarget(this, obj))
                    targets.Add(obj);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                // heavy physical + energy damage
                AOS.Damage(t, this, Utility.RandomMinMax(40, 60), 50, 0, 0, 0, 50);
                t.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        private void MutagenicBite()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*The raptor snaps with radioactive fangs!*");
                PlaySound(GetAttackSound());
                DoHarmful(target);

                // Bite damage
                int dmg = Utility.RandomMinMax(20, 35);
                AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

                // Apply poison
                target.ApplyPoison(this, Poison.Lethal);

                // Temporary stat reduction
                int drain = Utility.RandomMinMax(5, 15);
                target.Stam = Math.Max(0, target.Stam - drain);
                target.SendMessage(0x22, "You feel vitality draining from the radioactive wound!");
                target.FixedParticles(0x374A, 8, 12, 5032, UniqueHue, 0, EffectLayer.Waist);
            }
        }

		private void SpawnMinions()
		{
			this.Say("*Mutated young emerge!*");
			int count = Utility.RandomMinMax(1, 3);
			
			Mobile target = Combatant as Mobile;

			for (int i = 0; i < count; i++)
			{
				var baby = new Raptor(true);
				baby.Hue = UniqueHue;
				Point3D loc = this.Location;
				bool placed = false;

				for (int j = 0; j < 8 && !placed; j++)
				{
					int x = X + Utility.Random(-2, 2);
					int y = Y + Utility.Random(-2, 2);
					int z = Map.GetAverageZ(x, y);

					if (Map.CanFit(x, y, z, 16, false, false))
					{
						loc = new Point3D(x, y, z);
						placed = true;
					}
				}

				baby.MoveToWorld(loc, Map);

				if (baby.AIObject != null)
					baby.AIObject.Action = ActionType.Combat;

				if (target != null && SpellHelper.ValidIndirectTarget(this, target))
					baby.Combatant = target;
			}
		}


        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            // Radiological detonation on death
            this.Say("*Raaaaaaargh…*");
            Effects.PlaySound(Location, Map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3779, 15, 25, UniqueHue, 0, 5032, 0);

            // Chance for special "Enriched Raptor Claw"
            if (Utility.RandomDouble() < 0.02)
                c.DropItem(new RaptorClaw());  // assume this represents a nuclear‐infused claw
        }

        // Raptor sounds
        public override int GetIdleSound()  => 1573;
        public override int GetAngerSound() => 1570;
        public override int GetHurtSound()  => 1572;
        public override int GetDeathSound() => 1571;

        public override int TreasureMapLevel => 4;
        public override int Meat            => 7;
        public override int Hides           => 11;
        public override HideType HideType   => HideType.Horned;
        public override PackInstinct PackInstinct => PackInstinct.Ostard;

        public AtomicRaptor(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on load
            var now = DateTime.UtcNow;
            m_NextBlastTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextMutationTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSpawnTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation     = this.Location;
        }
    }
}
