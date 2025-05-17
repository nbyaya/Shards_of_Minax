using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // For Chain Lightning projectile IDs, if desired

namespace Server.Mobiles
{
    [CorpseName("a Minecraft Gargolye corpse")]
    public class MinecraftGargolye : BaseCreature
    {
        private const int UniqueHue = 1177; // A blocky emerald-green tint

        private DateTime m_NextCreeperTime;
        private DateTime m_NextBarrageTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        [Constructable]
        public MinecraftGargolye()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Minecraft Gargolye";
            Body = Utility.RandomList(666, 667);
            BaseSoundID = 362; // Gargoyle
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(350, 400);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(400, 550);

            SetDamage(20, 30);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 85.1, 95.0);
            SetSkill(SkillName.Wrestling, 85.1, 95.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 85;
            ControlSlots = 5;

            // ——— Initialize ability cooldowns ———
            m_NextCreeperTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextBarrageTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextSummonTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 80));

            m_LastLocation = this.Location;

            // ——— Loot ———
            PackItem(new IronIngot(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || !this.Alive)
                return;

            // Blocky dust effect when moving
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3728, 5, 10, UniqueHue, 0, 5039, 0);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !this.Alive)
                return;

            // Ensure Combatant is a Mobile before using Mobile-specific methods
            if (DateTime.UtcNow >= m_NextCreeperTime && Combatant is Mobile creeperTarget && this.InRange(creeperTarget.Location, 12) && CanBeHarmful(creeperTarget, false))
            {
                SpawnCreeperCharge(creeperTarget);
                m_NextCreeperTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextBarrageTime && Combatant is Mobile barrageTarget && this.InRange(barrageTarget.Location, 10) && CanBeHarmful(barrageTarget, false))
            {
                BlockBarrage(barrageTarget);
                m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextSummonTime && this.Hits < (this.HitsMax / 2) && this.Followers + 2 <= this.FollowersMax)
            {
                SummonBlockGolem();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(90, 120));
            }
        }

        // ——— Ability 1: Creeper Charge — spawn a LandmineTile under the target ———
        private void SpawnCreeperCharge(Mobile target)
        {
            this.Say("*ssss...*");
            PlaySound(0x2F5); // Hissing sound
            Point3D loc = target.Location;

            // Visual fuse effect
            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x374A, 8, 15, UniqueHue, 0, 5032, 0);

            // Place landmine hazard
            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                if (this.Map == null) return;
                if (!this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = this.Map.GetAverageZ(loc.X, loc.Y);

                var tile = new LandmineTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);
                Effects.PlaySound(loc, this.Map, 0x29A); // Explosion sound
            });
        }

        // ——— Ability 2: Block Barrage — hurl stone blocks at target ———
        private void BlockBarrage(Mobile target)
        {
            this.Say("*Take these blocks!*");
            PlaySound(0x20A); // Thunk of thrown projectile

            int shots = 5;
            for (int i = 0; i < shots; i++)
            {
                // Launch a single block projectile
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x1BFB, // Stone block graphic
                    7, 0, false, false, UniqueHue, 0, 0x1001, 1, 0, EffectLayer.Head, 0);

                int damage = Utility.RandomMinMax(30, 40);
                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (target.Alive && CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                        target.PlaySound(0x3E8);
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // ——— Ability 3: Summon Block Golem ———
        private void SummonBlockGolem()
        {
            this.Say("*Rise, my blocky servant!*");
            PlaySound(0x214);
            Point3D spawn = new Point3D(
                this.X + Utility.RandomMinMax(-2, 2),
                this.Y + Utility.RandomMinMax(-2, 2),
                this.Z);

            if (!this.Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                spawn.Z = this.Map.GetAverageZ(spawn.X, spawn.Y);

            var golem = new EarthElemental();
            golem.MoveToWorld(spawn, this.Map);
            golem.Combatant = this.Combatant;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            this.Say("*Blocks... crumble...*");
            Effects.PlaySound(this.Location, this.Map, 0x2A5); // Crumbling stone

            // Spawn pool of lava hazards around corpse
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(new Point3D(x, y, z), this.Map);
                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(x, y, z), this.Map, EffectItem.DefaultDuration),
                    0x3709, 8, 20, UniqueHue, 0, 5052, 0);
            }
        }

        // ——— Standard Overrides & Loot ———
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus    { get { return 75.0;  } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new PowerCrystal());  // Example unique drop
        }

        public MinecraftGargolye(Serial serial)
            : base(serial)
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

            // Reset timers on reload
            m_NextCreeperTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextBarrageTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextSummonTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 80));
        }
    }
}
