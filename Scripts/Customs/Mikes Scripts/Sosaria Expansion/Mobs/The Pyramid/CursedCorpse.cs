using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cursed corpse")]
    public class CursedCorpse : BaseCreature
    {
        // Cooldown timers for special attacks
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextBoneShowerTime;
        private DateTime m_NextSummonTime;

        // Unique sickly-green hue
        private const int UniqueHue = 2111;

        [Constructable]
        public CursedCorpse()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a cursed corpse";
            Body = 155;
            BaseSoundID = 471;
            Hue = UniqueHue;

            // —— Stats ——
            SetStr(350, 420);
            SetDex(80, 120);
            SetInt(200, 260);

            SetHits(1600, 1900);
            SetStam(200, 300);
            SetMana(100, 200);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 50);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextAuraTime        = now + TimeSpan.FromSeconds(8);
            m_NextNovaTime        = now + TimeSpan.FromSeconds(15);
            m_NextBoneShowerTime  = now + TimeSpan.FromSeconds(20);
            m_NextSummonTime      = now + TimeSpan.FromSeconds(30);
        }

        // —— Aura: Cursed Touch on Movement ——
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m == null || !Alive || m.Map != Map || !m.InRange(Location, 2) || !CanBeHarmful(m, false))
            {
                base.OnMovement(m, oldLocation);
                return;
            }

            if (SpellHelper.ValidIndirectTarget(this, m))
            {
                DoHarmful(m);
                m.SendMessage(0x22, "You feel a malefic curse cling to you!");
                m.FixedParticles(0x374A, 8, 12, 5032, UniqueHue, 0, EffectLayer.Head);
                AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 20, 80);
            }

            base.OnMovement(m, oldLocation);
        }

        // —— Core AI Loop ——  
        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;
            var target = Combatant as Mobile;

            if (target != null && CanBeHarmful(target, false))
            {
                double dist = GetDistanceToSqrt(target);

                // 1) Curse Nova — AoE damage + stat-cripple
                if (now >= m_NextNovaTime && dist <= 6)
                {
                    CurseNova();
                    m_NextNovaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
                // 2) Bone Shower — raining projectiles on a single target
                else if (now >= m_NextBoneShowerTime && dist <= 12)
                {
                    BoneShower(target);
                    m_NextBoneShowerTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
                // 3) Summon Wailing Spirits — calls 2–4 skeletons/ghosts
                else if (now >= m_NextSummonTime)
                {
                    SummonMinions();
                    m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
                }
            }
        }

        // —— Curse Nova: Huge AoE + temporary stat drain ——
        private void CurseNova()
        {
            PlaySound(0x20A);
            FixedParticles(0x3709, 12, 20, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, dmg, 0, 0, 0, 30, 70);

                // Apply a brief Strength/DEX curse via StatMod and remove after 10s
                int strDrain = Utility.RandomMinMax(5, 10);
                int dexDrain = Utility.RandomMinMax(5, 10);

                string strKey = $"{m.Serial.Value}_CurseStr";
                string dexKey = $"{m.Serial.Value}_CurseDex";

                // Add the stat mods
                m.AddStatMod(new StatMod(StatType.Str, strKey, -strDrain, TimeSpan.Zero));   // manual removal
                m.AddStatMod(new StatMod(StatType.Dex, dexKey, -dexDrain, TimeSpan.Zero));   // manual removal :contentReference[oaicite:0]{index=0}

                // Schedule removal
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                {
                    m.RemoveStatMod(strKey);
                    m.RemoveStatMod(dexKey);
                });

                m.SendMessage(0x22, "Your limbs feel weakened by the curse!");
            }
        }

        // —— Bone Shower: Targeted volley of bone shards ——
        private void BoneShower(Mobile target)
        {
            Say("*Rattle... rattle...*");
            PlaySound(0x48D);

            for (int i = 0; i < 8; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(100 * i), () =>
                {
                    if (target.Alive && CanBeHarmful(target, false))
                    {
                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, Location, Map),
                            new Entity(Serial.Zero, target.Location, target.Map),
                            0x36A4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

                        DoHarmful(target);
                        AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        // —— Summon 2–4 skeletal minions ——
        private void SummonMinions()
        {
            Say("*Rise… my children!*");
            PlaySound(0x212);

            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                var spawn = new Skeleton(); // or your custom skeleton
                spawn.MoveToWorld(new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z), Map);
                spawn.Combatant = Combatant;
            }
        }

        // —— Death: Necrotic Explosion + Hazard Tiles ——
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*…and decay claims all!*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 30, UniqueHue, 0, 5052, 0);

                // Scatter necromantic flamestrike hazards
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = new NecromanticFlamestrikeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // —— Loot & Properties ——
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison    => Poison.Lethal;
        public override int TreasureMapLevel => 6;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03) // 3% chance for special artifact
                PackItem(new NecroticSash());
        }

        public CursedCorpse(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers
            var now = DateTime.UtcNow;
            m_NextAuraTime        = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextNovaTime        = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBoneShowerTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSummonTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
        }
    }
}
