using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a ceratopsian construct corpse")]
    public class VaultCeratops3 : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextPrimalRoar;
        private DateTime m_NextEarthenRupture;
        private DateTime m_NextAcidSpit;
        private Point3D m_LastLocation;

        // Unique emerald‑green glow
        private const int UniqueHue = 1324;

        [Constructable]
        public VaultCeratops3()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Vault Ceratops-3";
            Body = 0x587;
            BaseSoundID = 0x672;  // use hurt sound as base
            Hue = UniqueHue;

            // —— Core Stats ——
            SetStr(1500, 1800);
            SetDex(300, 350);
            SetInt(400, 450);

            SetHits(2000, 2400);
            SetStam(400, 500);
            // Minimal mana; this is purely a melee/brute boss
            SetMana(0);

            SetDamage(30, 40);

            // —— Damage Types ——
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 60, 70);

            // —— Skills ——
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Parry, 100.0, 110.0);
            SetSkill(SkillName.Focus, 95.0, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            DateTime now = DateTime.UtcNow;
            m_NextPrimalRoar   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEarthenRupture = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAcidSpit     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new Bone(Utility.RandomMinMax(5, 10)));
            PackItem(new Diamond(Utility.RandomMinMax(2, 4)));
            AddLoot(LootPack.UltraRich, 2);
        }

        // —— Sounds —— (reuse Triceratops sounds)
        public override int GetIdleSound()  { return 0x673; }
        public override int GetAngerSound() { return 0x670; }
        public override int GetHurtSound()  { return 0x672; }
        public override int GetDeathSound() { return 0x671; }

        // —— Movement Aura: Spawns seismic fissures behind it ——
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Every time it moves, 20% chance to leave an EarthquakeTile
            if (this.Map != null && this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var loc = m_LastLocation;
                int z = Map.GetAverageZ(loc.X, loc.Y);
                if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !this.Alive || this.Map == null || this.Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // Primal Roar: fear & stagger in 8‑tile cone
            if (now >= m_NextPrimalRoar && InRange(Combatant.Location, 8))
            {
                DoPrimalRoar();
                m_NextPrimalRoar = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Earthen Rupture: AoE shockwave + LandmineTiles
            else if (now >= m_NextEarthenRupture && InRange(Combatant.Location, 6))
            {
                DoEarthenRupture();
                m_NextEarthenRupture = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Acid Spit: ranged poison attack
            else if (now >= m_NextAcidSpit && InRange(Combatant.Location, 12))
            {
                DoAcidSpit();
                m_NextAcidSpit = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
        }

        // —— Primal Roar —— fears and staggers targets in a cone
        private void DoPrimalRoar()
        {
            this.Say("*ROOOAAAR!*");
            this.PlaySound(0x213);

            var origin = this.Location;
            IPooledEnumerable eable = Map.GetMobilesInRange(origin, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target)
                {
                    // simple distance check; for a true cone you'd check angles
                    DoHarmful(target);
                    target.SendMessage("You are stunned by the Ceratops' mighty roar!");
                    target.Freeze(TimeSpan.FromSeconds(2.0));
                }
            }
            eable.Free();
        }

        // —— Earthen Rupture —— AoE physical damage + mines
        private void DoEarthenRupture()
        {
            this.Say("*The ground shatters!*");
            this.PlaySound(0x2A3);

            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 10, 30, UniqueHue);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target)
                    targets.Add(target);
            }
            eable.Free();

            foreach (var target in targets)
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
            }

            // Scatter landmines
            for (int i = 0; i < 5; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + dx, Y + dy, Z);
                int z = Map.GetAverageZ(loc.X, loc.Y);
                if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
                }
            }
        }

        // —— Acid Spit —— ranged poison projectile
        private void DoAcidSpit()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Graaak!*");
            this.PlaySound(0x224);

            // Projectile effect
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x1FBC, 7, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (target.Map == this.Map)
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);
                    target.SendMessage("Acid burns your flesh!");
                    target.ApplyPoison(this, Poison.Lethal);
                }
            });
        }

        // —— Death: erupting acid pools —— 
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*The Ceratops collapses in a fountain of acid!*");
            this.PlaySound(0x211);

            // Spawn several PoisonTiles around the corpse
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                int z = Map.GetAverageZ(loc.X, loc.Y);
                if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var pool = new PoisonTile();
                    pool.Hue = UniqueHue;
                    pool.MoveToWorld(new Point3D(loc.X, loc.Y, z), this.Map);
                }
            }
        }

        // —— Standard Overrides ——
        public override bool BleedImmune   => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.05) // 5% chance for unique vault key
                PackItem(new StepOfTheGladewalker());
        }

        public VaultCeratops3(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            DateTime now = DateTime.UtcNow;
            m_NextPrimalRoar     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEarthenRupture = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextAcidSpit       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
        }
    }
}
