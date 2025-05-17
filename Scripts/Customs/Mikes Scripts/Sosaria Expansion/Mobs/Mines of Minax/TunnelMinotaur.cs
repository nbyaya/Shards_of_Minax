using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tunnel minotaur corpse")]
    public class TunnelMinotaur : BaseCreature
    {
        private DateTime m_NextStampede;
        private DateTime m_NextQuake;
        private DateTime m_NextLabyrinth;
        private Point3D m_LastLocation;

        // A deep, earthen red
        private const int UniqueHue = 1899;

        [Constructable]
        public TunnelMinotaur()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "a Tunnel Minotaur";
            Body = 281;
            Hue  = UniqueHue;

            // ——— Stats ———
            SetStr(600, 700);
            SetDex(150, 200);
            SetInt( 80, 100);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(  0);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison,   40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   30, 40);

            // ——— Skills ———
            SetSkill(SkillName.Tactics,      110.1, 120.0);
            SetSkill(SkillName.Wrestling,    110.1, 120.0);
            SetSkill(SkillName.MagicResist,  100.1, 110.0);

            Fame       = 25000;
            Karma      = -25000;
            VirtualArmor = 90;
            ControlSlots = 3;

            // Initialize ability cooldowns
            m_NextStampede  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuake     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextLabyrinth = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation  = this.Location;

            // Thematic loot
            PackGold(2000, 3000);
            PackItem(new RootsingerLeggings());
        }

        // ——— Movement Aura: Spore‑Poison Cloud ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && Alive && m.Alive && m.InRange(this, 3) && CanBeHarmful(m, false))
            {
                if (m is Mobile target && Utility.RandomDouble() < 0.2) // 20% chance per step
                {
                    DoHarmful(target);
                    target.ApplyPoison(this, Poison.Regular);
                    target.SendMessage("You inhale poisonous spores!");
                    target.FixedParticles(0x376A, 10, 16, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // ——— AI Think: Trigger Special Abilities ———
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                var now = DateTime.UtcNow;

                if (now >= m_NextStampede && this.InRange(target.Location, 12))
                {
                    StampedeCharge(target);
                    m_NextStampede = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }
                else if (now >= m_NextQuake)
                {
                    EarthShake();
                    m_NextQuake = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                }
                else if (now >= m_NextLabyrinth)
                {
                    SummonLabyrinthTrap(target);
                    m_NextLabyrinth = now + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 50));
                }
            }
        }

		private Point3D GetPointCloser(Point3D target, int distance)
		{
			Map map = this.Map;

			if (map == null)
				return this.Location;

			// Direction vector
			int dx = target.X - X;
			int dy = target.Y - Y;

			double length = Math.Sqrt(dx * dx + dy * dy);
			if (length == 0)
				return this.Location;

			// Normalize and scale
			double scale = distance / length;

			int newX = X + (int)(dx * scale);
			int newY = Y + (int)(dy * scale);

			int newZ = map.GetAverageZ(newX, newY);

			return new Point3D(newX, newY, newZ);
		}


        // ——— Ability: Stampede Charge ———
        private void StampedeCharge(Mobile target)
        {
            Say("*RAAAAR!*");
            PlaySound(GetAttackSound());

            // Leap close to target:
            var dest = GetPointCloser(target.Location, 1);
            if (Map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
                this.MoveToWorld(dest, this.Map);

            DoHarmful(target);
            int dmg = Utility.RandomMinMax(50, 75);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

            // Stamina drain
            if (target is Mobile t) t.Stam = Math.Max(0, t.Stam - Utility.RandomMinMax(20, 30));

            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 10, 15, UniqueHue, 0, 5023, 0
            );
        }

        // ——— Ability: Earth Shake ———
        private void EarthShake()
        {
            Say("*The tunnels tremble!*");
            PlaySound(0x58F);

            // Spawn earthquake tiles in a 3‑tile radius
            for (int dx = -3; dx <= 3; dx++)
            {
                for (int dy = -3; dy <= 3; dy++)
                {
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }
        }

        // ——— Ability: Summon Labyrinth Trap ———
        private void SummonLabyrinthTrap(Mobile target)
        {
            Say("*Get lost in the labyrinth!*");
            PlaySound(0x1F6);

            var loc = target.Location;
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                // Try to drop a quicksand tile under the target
                var spawnZ = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, spawnZ, 16, false, false))
                    spawnZ = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, spawnZ, 16, false, false))
                {
                    var trap = new QuicksandTile();
                    trap.Hue = UniqueHue;
                    trap.MoveToWorld(new Point3D(loc.X, loc.Y, spawnZ), Map);
                }
            });
        }

        // ——— Death Throes: Landmine Cage ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Say("*The labyrinth... collapses...*");
            Effects.PlaySound(Location, Map, 0x58F);

            // Scatter 3–6 landmines around the corpse
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + dx, Y + dy, Z);

                var z = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                }
            }
        }

        // ——— Standard Overrides ———
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override int GetAngerSound()  { return 0x597; }
        public override int GetIdleSound()   { return 0x596; }
        public override int GetAttackSound() { return 0x599; }
        public override int GetHurtSound()   { return 0x59A; }
        public override int GetDeathSound()  { return 0x59C; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,       Utility.RandomMinMax(5, 10));

            // 1% chance for a Horn of the Labyrinth
            if (Utility.RandomDouble() < 0.01)
                PackItem(new HornOfTheLabyrinth());
        }

        // ——— Serialization ———
        public TunnelMinotaur(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers so abilities don’t all trigger at once after a restart
            m_NextStampede  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuake     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextLabyrinth = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}
