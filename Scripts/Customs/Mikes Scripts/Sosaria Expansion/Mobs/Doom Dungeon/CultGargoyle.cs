using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a corrupt gargoyle corpse")]
    public class CultGargoyle : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextScreamTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextShardVolleyTime;
        private Point3D m_LastLocation;

        // Unique hue – deep blood‑red
        private const int CultHue = 1175;

        [Constructable]
        public CultGargoyle() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Cult Gargoyle";
            Body = 0x2F3;              // same gargoyle form
            BaseSoundID = 0x174;       // same gargoyle sounds
            Hue = CultHue;

            // Core stats
            SetStr(900, 1000);
            SetDex(120, 180);
            SetInt(200, 250);

            SetHits(600, 700);
            SetStam(200, 250);
            SetMana(300, 350);

            // Damage – mix of physical and necrotic
            SetDamage(12, 18);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 50);

            // Skills
            SetSkill(SkillName.Wrestling, 100.1, 115.0);
            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 80;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextScreamTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextShardVolleyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            // Basic loot
            PackItem(new GargoylesPickaxe());
            PackItem(new Bloodmoss(Utility.RandomMinMax(5, 10)));
            PackGold(500, 750);

            m_LastLocation = this.Location;
        }

        public override bool CanFly => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 145.0;
        public override double DispelFocus => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));
            // 3% chance for a cursed totem
            if (Utility.RandomDouble() < 0.03)
                PackItem(new DruidsRootbind());
        }

        // Aura on movement: leave toxic gas tiles
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                Point3D dropLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = CultHue;
                gas.MoveToWorld(dropLoc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Necrotic Scream – AoE fear + damage every ~15s
            if (now >= m_NextScreamTime && InRange(Combatant.Location, 6))
            {
                NecroticScream();
                m_NextScreamTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Summon Gargoyle Acolytes every ~25s
            else if (now >= m_NextSummonTime)
            {
                SummonAcolytes();
                m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            // Corrupted Shard Volley – ranged attack every ~10s
            else if (now >= m_NextShardVolleyTime)
            {
                CorruptedShardVolley();
                m_NextShardVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        private void NecroticScream()
        {
            Say("*Screeeee echo of the damned!*");
            PlaySound(0x2F5);
            var ents = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    ents.Add(m);
            }

            foreach (var m in ents)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                if (m is Mobile target)
                {
                    target.SendMessage(0x22, "You are terrified by the gargoyle's unholy scream!");
                    SpellHelper.AddStatCurse(this, target, StatType.Str);
                    target.FixedParticles(0x3728, 10, 15, 5032, CultHue, 0, EffectLayer.Head);
                }
            }
        }

        private void SummonAcolytes()
        {
            Say("*Rise, my brethren!*");
            for (int i = 0; i < Utility.RandomMinMax(2, 4); i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomList(-2, -1, 1, 2),
                    Y + Utility.RandomList(-2, -1, 1, 2),
                    Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var acolyte = new Gargoyle();
                acolyte.MoveToWorld(loc, Map);
            }
        }

        private void CorruptedShardVolley()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Shards of ruin!*");
            PlaySound(0x1FE);

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    if (!(Combatant is Mobile t) || !CanBeHarmful(t, false)) return;

                    // Visual
                    Effects.SendMovingParticles(
                        new Entity(Serial.Zero, Location, Map),
                        new Entity(Serial.Zero, t.Location, Map),
                        0x1BFB, 7, 0, false, false, CultHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                    // Damage
                    DoHarmful(t);
                    AOS.Damage(t, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);

                    // Chance to curse stamina
                    if (Utility.RandomDouble() < 0.3 && t is Mobile tm)
                    {
                        tm.Stam = Math.Max(0, tm.Stam - Utility.RandomMinMax(10, 20));
                        tm.SendMessage(0x22, "The shard saps your strength!");
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My brethren... avenge me...*");
                PlaySound(0x2F6);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 12, 60, CultHue, 0, 5052, 0);

                // Spawn a few venomous ground hazards
                for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomList(-3, -2, -1, 1, 2, 3),
                        Y + Utility.RandomList(-3, -2, -1, 1, 2, 3),
                        Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = new PoisonTile();
                    tile.Hue = CultHue;
                    tile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public CultGargoyle(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            // Reset timers
            m_NextScreamTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextShardVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_LastLocation        = this.Location;
        }
    }
}
