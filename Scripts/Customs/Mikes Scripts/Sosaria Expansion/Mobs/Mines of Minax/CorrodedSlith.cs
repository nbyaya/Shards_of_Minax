using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a corroded slith corpse")]
    public class CorrodedSlith : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime _nextSpitTime;
        private DateTime _nextBurstTime;
        private DateTime _nextAuraTime;
        private Point3D _lastLocation;
        
        // Unique sickly patina green
        private const int UniqueHue = 2117;

        [Constructable]
        public CorrodedSlith()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a corroded slith";
            Body = 734;                   // Same hulking slith body
            BaseSoundID = 0xDB;           // Hissing slith sound
            Hue = UniqueHue;

            // Stats
            SetStr(400, 450);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(300, 400);

            SetDamage(20, 30);

            // Damage profile: mostly acid/poison
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.Poisoning, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize ability timers
            var now = DateTime.UtcNow;
            _nextSpitTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextAuraTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));

            _lastLocation = this.Location;

            // Loot
            PackItem(new NecromanticFlamestrikeTile()); // Rare crafting tile
            PackItem(new GreaterPoisonPotion());
        }

        // --- Acidic Trail: leave toxic gas as it moves ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Deleted || !this.Alive) return;

            if (this.Location != _lastLocation && Utility.RandomDouble() < 0.3)
            {
                var loc = _lastLocation;
                if (this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, this.Map);
                }
            }

            _lastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || this.Map == null || !this.Alive)
                return;

            var now = DateTime.UtcNow;

            // Corrosive Aura: weakens armor of close targets
            if (now >= _nextAuraTime)
            {
                if (this.InRange(Combatant.Location, 3) && Combatant is Mobile auraTarget)
                {
                    if (CanBeHarmful(auraTarget, false))
                    {
                        DoHarmful(auraTarget);
                        auraTarget.SendMessage("You feel the metal of your armor corroding!");
                        // Temporarily reduce physical resistance
                        auraTarget.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
                        // Simulate corrosion by reducing Dex (affects AC)
                        auraTarget.Dex -= 10;
                        Timer.DelayCall(TimeSpan.FromSeconds(8), () => { auraTarget.Dex += 10; });
                    }
                }
                _nextAuraTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            }

            // Acidic Spit: ranged targeted acid hazard
            if (now >= _nextSpitTime && this.InRange(Combatant.Location, 12))
            {
                AcidSpit(Combatant);
                _nextSpitTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Corrosive Burst: AoE poison/acid tiles
            else if (now >= _nextBurstTime && this.InRange(Combatant.Location, 6))
            {
                CorrosiveBurst();
                _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Acidic Spit: spit an acid projectile ---
		private void AcidSpit(IDamageable targetDamageable)
		{
			if (this.Map == null) return;
			if (!(targetDamageable is Mobile target) || !CanBeHarmful(target, false))
				return;

			this.Say("*Screee*");
			PlaySound(0x227);

			// Send acid projectile from slith to target
			Effects.SendMovingEffect(this, target, 0x36E4, 10, 1, false, false, UniqueHue, 0);

			// Delay impact
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
			{
				if (!this.Alive || target.Deleted) return;

				DoHarmful(target);
				AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
				target.FixedParticles(0x373A, 8, 20, 5032, UniqueHue, 0, EffectLayer.Head);
				target.PlaySound(0x658);

				// Spawn a small poison tile at their feet
				var loc = target.Location;
				if (this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
				{
					var tile = new PoisonTile();
					tile.Hue = UniqueHue;
					tile.MoveToWorld(loc, this.Map);
				}
			});
		}


        // --- Corrosive Burst: ground littered with acid hazards ---
        private void CorrosiveBurst()
        {
            if (this.Map == null) return;

            this.PlaySound(0x211);
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Point3D>();
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                    list.Add(new Point3D(this.X + dx * 2, this.Y + dy * 2, this.Z));

            foreach (var loc in list)
            {
                if (this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new ToxicGasTile();  // your own custom acid ground tile
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                }
            }
        }

        // --- Death Explosion: spawn hazards around corpse ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (this.Map == null) return;
            this.Say("*Rsssss...*");
            Effects.PlaySound(this.Location, this.Map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 20, 40, UniqueHue, 0, 5031, 0);

            for (int i = 0; i < 6; i++)
            {
                int x = this.X + Utility.RandomMinMax(-3, 3);
                int y = this.Y + Utility.RandomMinMax(-3, 3);
                int z = this.Z;
                var loc = new Point3D(x, y, z);

                if (!this.Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = this.Map.GetAverageZ(x, y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, this.Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new TrapWeb());
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus => 60.0;

        public CorrodedSlith(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers
            var now = DateTime.UtcNow;
            _nextSpitTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            _nextBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            _nextAuraTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            _lastLocation  = this.Location;
        }
    }
}
