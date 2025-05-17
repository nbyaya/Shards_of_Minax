using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For spell effects
using Server.Network;        // For visual and sound effects
using System.Collections.Generic;
using Server.Spells.Fourth;   // For FireField spell effects

namespace Server.Mobiles
{
    [CorpseName("a scorching llama corpse")]
    public class ScorchingLlama : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextChargeTime;
        private DateTime m_NextHeatWaveTime;
        private Point3D m_LastLocation;

        // Unique hue for this fiery beast (bright fire orange/red)
        private const int UniqueHue = 1161;

        [Constructable]
        public ScorchingLlama() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scorching Llama";
            // Use the same body and base sound as the standard llama:
            Body = 0xDC; 
            BaseSoundID = 0x3F3;
            Hue = UniqueHue;

            // --- Advanced Stats (Boss-level creature) ---
            SetStr(450, 550);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(1500, 1700);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(30, 40);
            // Deals a mix of physical and dominant fire damage
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // Resistances: very high fire resistance, moderate physical, weak cold, etc.
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills ---
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 3;
			Tamable = true; // This is a boss-level creature

            // Initialize ability cooldowns
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextHeatWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_LastLocation = this.Location;

            // Standard loot (with extra fire-themed ingredients)
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));
        }

        // --- Scorching Aura: leave a brief flame field behind when moving ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && m.InRange(this.Location, 2))
            {
                // Create a short-duration fire field at the mobile's previous location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and sound effect for the trail
                m.FixedParticles(0x3709, 10, 20, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Main Thinking Loop ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a burning trail if we moved
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D trailLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, trailLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure Combatant is a Mobile before proceeding
            Mobile target = Combatant as Mobile;
            if (target == null)
                return;

            // --- Flame Charge Attack ---
            // When target is within 10 tiles and the cooldown has expired, perform a powerful charge attack.
            if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(target.Location, 10))
            {
                FlameChargeAttack(target);
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // --- Heat Wave Attack ---
            // If target is close (within 8 tiles) and timer expired, release an AoE burst of sizzling heat.
            else if (DateTime.UtcNow >= m_NextHeatWaveTime && this.InRange(target.Location, 8))
            {
                HeatWaveAttack();
                m_NextHeatWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
        }

        // --- Flame Charge Attack ---
        // Dashes toward the target, leaving a trail of flames and delivering burst fire damage.
        private void FlameChargeAttack(Mobile target)
        {
            if (target == null || Map == null)
                return;

            this.Say("*Flame Charge!*");
            PlaySound(0x160); // Use a charge/explosion sound

            // Display a moving particle effect from the current location to the target
            Point3D startPoint = this.Location;
            Point3D endPoint = target.Location;
            Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, endPoint, this.Map),
                0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            // Gather nearby mobiles along the start point; these represent foes caught in the charge.
            List<Mobile> affected = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(startPoint, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    affected.Add(m);
            }
            eable.Free();

            foreach (Mobile m in affected)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(35, 50);
                // Apply fire-only damage
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                // Show a burning hit effect on each target
                m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
            }
        }

        // --- Heat Wave Attack ---
        // Emits a powerful burst of searing heat around itself that damages all nearby enemies.
        private void HeatWaveAttack()
        {
            if (Map == null)
                return;

            this.Say("*Heat Wave!*");
            PlaySound(0x208);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Pure fire damage
                m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
            }
        }

        // --- Death Explosion ---
        // Upon death, spawns several hot lava tiles and produces a grand explosion effect.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 8;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 55.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));

            if (Utility.RandomDouble() < 0.01)
                PackItem(new InfernosEmbraceCloak());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(2, 4)));
        }

        public ScorchingLlama(Serial serial) : base(serial)
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
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextHeatWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
        }
    }
}
