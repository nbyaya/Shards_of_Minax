using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For spell effects if needed
using Server.Network;        // For visual and sound effects

namespace Server.Mobiles
{
    [CorpseName("a venom elemental corpse")]
    public class ToxinElemental : BaseCreature
    {
        // --- Timers for special abilities ---
        private DateTime m_NextToxicMiasmaTime;
        private DateTime m_NextCorrosiveBurstTime;
        private DateTime m_NextVenomTorrentTime;
        private Point3D m_LastLocation;

        // Unique hue for this monster (a virulent greenish tone)
        private const int UniqueHue = 1200;

        [Constructable]
        public ToxinElemental() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Venom Elemental";
            Body = 162;
            BaseSoundID = 263;
            Hue = UniqueHue;

            // --- Adjusted and Enhanced Stats ---
            SetStr(600, 650);
            SetDex(250, 300);
            SetInt(700, 750);

            SetHits(1600, 2000);
            SetStam(300, 350);
            SetMana(700, 850);

            SetDamage(15, 25);

            // Damage: Mostly poison with a little physical
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            // --- Skills (Magic & Poison Focus) ---
            SetSkill(SkillName.EvalInt, 120.1, 135.0);
            SetSkill(SkillName.Magery, 120.1, 135.0);
            SetSkill(SkillName.MagicResist, 120.2, 135.0);
            SetSkill(SkillName.Poisoning, 120.1, 135.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 105.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.DetectHidden, 80.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextToxicMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorrosiveBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextVenomTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // --- Basic Loot ---
            PackItem(new Nightshade(Utility.RandomMinMax(6, 10)));
            PackItem(new LesserPoisonPotion(Utility.RandomMinMax(2, 4)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // --- Toxic Trail Effect: Leave a puddle of Toxic Gas where you were ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    ToxicGasTile toxicTile = new ToxicGasTile();
                    toxicTile.Hue = UniqueHue;
                    toxicTile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        ToxicGasTile toxicTile = new ToxicGasTile();
                        toxicTile.Hue = UniqueHue;
                        toxicTile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // --- Ability Usage ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities (highest: Venomous Torrent, then Corrosive Burst, then Toxic Miasma)
            if (DateTime.UtcNow >= m_NextVenomTorrentTime && this.InRange(Combatant.Location, 10))
            {
                ChainVenomTorrentAttack();
                m_NextVenomTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            else if (DateTime.UtcNow >= m_NextCorrosiveBurstTime && this.InRange(Combatant.Location, 8))
            {
                CorrosiveBurstAttack();
                m_NextCorrosiveBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextToxicMiasmaTime && this.InRange(Combatant.Location, 6))
            {
                ToxicMiasmaAttack();
                m_NextToxicMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // --- Ability: Toxic Miasma ---
        // Emits a poisonous cloud that damages and may poison nearby foes.
        public void ToxicMiasmaAttack()
        {
            this.Say("*A noxious miasma emanates from the venom!*");
            PlaySound(0x1F2); // Example: a hissing, poisonous sound
            this.FixedParticles(0x374A, 10, 20, 5045, UniqueHue, 0, EffectLayer.Waist); // Visual poison cloud effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(20, 30);
                // Deal 100% poison damage (0 physical, 0 fire, 0 cold, 100 poison, 0 energy)
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);

                // 50% chance to further apply lethal poison
                if (Utility.RandomDouble() < 0.50)
                {
                    if (target is Mobile targetMobile)
                    {
                        targetMobile.ApplyPoison(this, Poison.Lethal);
                        targetMobile.SendMessage(0x22, "The toxic miasma burns your veins!");
                    }
                }
            }
        }

        // --- Ability: Corrosive Burst ---
        // Releases a damaging burst that deals area poison damage and may inflict poison.
        public void CorrosiveBurstAttack()
        {
            if (Map == null)
                return;

            this.Say("*Venomous corrosion erupts!*");
            PlaySound(0x20F); // Sound for corrosive explosion
            this.FixedParticles(0x3709, 10, 30, 5050, UniqueHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 50);
                    AOS.Damage(target, this, damage, 0, 0, 0, 100, 0); // 100% poison damage

                    // 40% chance to apply an extra dose of lethal poison
                    if (Utility.RandomDouble() < 0.40)
                    {
                        if (target is Mobile targetMobile)
                        {
                            targetMobile.ApplyPoison(this, Poison.Lethal);
                            targetMobile.SendMessage(0x22, "Corrosive venom seeps into your wounds!");
                        }
                    }
                }
            }
        }

        // --- Ability: Chain Venom Torrent ---
        // Launches a chain attack that bounces between foes applying poison damage.
        public void ChainVenomTorrentAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the toxic surge!*");
            PlaySound(0x20A); // Sound for the chain attack

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;
            targets.Add(currentTarget);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 100, 0); // Poison damage
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);

                        if (Utility.RandomDouble() < 0.40)
                        {
                            if (damageTarget is Mobile targetMobile)
                            {
                                targetMobile.ApplyPoison(this, Poison.Lethal);
                                targetMobile.SendMessage(0x22, "You are struck by searing venom!");
                            }
                        }
                    }
                });
            }
        }

        // --- Death Effect: Venomous Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Venom... unleashed!*");
                PlaySound(0x211); // Poison explosion sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5050, 0);

                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    PoisonTile poisonTile = new PoisonTile();
                    poisonTile.Hue = UniqueHue;
                    poisonTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override double HitPoisonChance { get { return 0.75; } }
        public override int TreasureMapLevel { get { return 7; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 80.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new VenomlordsShroud());

        }

        public ToxinElemental(Serial serial) : base(serial)
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

            m_NextToxicMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorrosiveBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextVenomTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
