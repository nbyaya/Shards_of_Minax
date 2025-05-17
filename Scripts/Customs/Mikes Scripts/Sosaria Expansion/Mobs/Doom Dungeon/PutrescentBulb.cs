using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a putrescent bulbous corpse")]
    public class PutrescentBulb : BaseCreature
    {
        private DateTime m_NextAuraTime;
        private DateTime m_NextSporeTime;
        private DateTime m_NextBarrageTime;
        private DateTime m_NextSummonTime;
        private const int UniqueHue = 1163; // Sickly green

        [Constructable]
        public PutrescentBulb()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a putrescent bulb";
            Body = 0x307;
            BaseSoundID = 0x165;
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(800, 900);
            SetDex(80, 100);
            SetInt(70, 90);

            SetHits(2000, 2300);
            SetStam(150, 200);
            SetMana(200, 250);

            SetDamage(30, 40);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   50, 60);

            // ——— Skills ———
            SetSkill(SkillName.Wrestling,     100.0, 115.0);
            SetSkill(SkillName.Tactics,       100.0, 115.0);
            SetSkill(SkillName.MagicResist,    90.0, 105.0);
            SetSkill(SkillName.Poisoning,     120.0, 130.0);
            SetSkill(SkillName.Anatomy,       100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // ——— Ability Cooldowns ———
            m_NextAuraTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5,  8));
            m_NextSporeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // ——— Starter Loot ———
            PackItem(new BlackPearl(Utility.RandomMinMax(15,20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15,20)));
            PackItem(new Nightshade(Utility.RandomMinMax(3,6))); // ← now resolves
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison    { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // ——— Putrescent Aura ———
            if (now >= m_NextAuraTime)
            {
                m_NextAuraTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x373A, 8, 20, UniqueHue, 0, 5034, 0);
                PlaySound(0x208);

                foreach (Mobile m in Map.GetMobilesInRange(Location, 3))
                {
                    if (m != this && m.Alive && CanBeHarmful(m, false))
                    {
                        DoHarmful(m);
                        int dmg = Utility.RandomMinMax(20, 30);
                        AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                        m.SendMessage("You’re scorched by its noxious aura!");

                        if (Utility.RandomDouble() < 0.5)
                            m.ApplyPoison(this, Poison.Lethal);  // ← two args: source + poison
                    }
                }
            }

            // ——— Spore Eruption ———
            if (now >= m_NextSporeTime)
            {
                m_NextSporeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

                if (Combatant is Mobile target && InRange(target.Location, 10))
                {
                    Say("Spore eruption!");
                    PlaySound(0x20C);

                    for (int i = 0; i < 5; i++)
                    {
                        int dx = Utility.RandomMinMax(-3, 3);
                        int dy = Utility.RandomMinMax(-3, 3);
                        Point3D loc = new Point3D(X + dx, Y + dy, Z);

                        if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                            loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                        PoisonTile tile = new PoisonTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }

            // ——— Spore Barrage ———
            if (now >= m_NextBarrageTime && InRange(Combatant.Location, 12))
            {
                m_NextBarrageTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));

                if (Combatant is Mobile target && CanBeHarmful(target, false))
                {
                    Say("Feel my spores!");
                    PlaySound(0x1FE);

                    Effects.SendMovingParticles(
                        new Entity(Serial.Zero, Location, Map),
                        new Entity(Serial.Zero, target.Location, Map),
                        0x36D4, 5, 0, false, false,
                        UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

                    Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
                    {
                        if (CanBeHarmful(target, false))
                        {
                            DoHarmful(target);
                            int dmg = Utility.RandomMinMax(40, 60);
                            AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

                            if (Utility.RandomDouble() < 0.3)
                                target.ApplyPoison(this, Poison.Lethal);  // ← fixed here too
                        }
                    });
                }
            }

            // ——— Summon Putrid Spawns ———
            if (now >= m_NextSummonTime)
            {
                m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

                if (Combatant is Mobile target)
                {
                    Say("Arise, my children!");
                    for (int i = 0; i < 2; i++)
                    {
                        var minion = new BulbousPutrification();
                        minion.MoveToWorld(Location, Map);
                        minion.Combatant = target;
                    }
                }
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                PlaySound(0x207);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x374A, 10, 30, UniqueHue, 0, 5032, 0);

                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-4, 4);
                    int dy = Utility.RandomMinMax(-4, 4);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                        if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                            continue;
                    }

                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new RotweaversCloak());
        }

        public PutrescentBulb(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextAuraTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5,  8));
            m_NextSporeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
