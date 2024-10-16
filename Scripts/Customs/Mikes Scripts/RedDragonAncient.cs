using Server.Items;
using Server.Network;
using Server.Spells.Fourth;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a ancient red wyrm corpse")]
    public class RedDragonAncient : BaseCreature
    {
        private DateTime m_NextBreathTime;
        private DateTime _nextTrueFearAllowed;
        private static readonly TimeSpan TrueFearCooldown = TimeSpan.FromSeconds(30); // Cooldown duration
        private Timer m_DisguiseTimer;
        private DateTime _nextNukeAllowed;
        private static readonly TimeSpan NukeCooldown = TimeSpan.FromSeconds(25); // 25-second cooldown


        [Constructable]
        public RedDragonAncient()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "a Ancient Red Wyrm";
            Body = 826;
            Hue = 1645;
            BaseSoundID = 362;

            SetStr(1185, 1500);
            SetDex(86, 175);
            SetInt(775, 1000);

            SetHits(4500, 5000);
            SetDamage(30, 45);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 100, 150);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 115, 130);
            SetSkill(SkillName.Meditation, 52.5, 75.0);
            SetSkill(SkillName.MagicResist, 100.5, 125.0);
            SetSkill(SkillName.Tactics, 120, 125.0);
            SetSkill(SkillName.Wrestling, 120, 125.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            SetWeaponAbility(WeaponAbility.Dismount);

            ForceActiveSpeed = 0.3;
            ForcePassiveSpeed = 0.6;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && !willKill && amount > 5 && from.Player && Utility.RandomDouble() < 0.2) // 20% chance
            {
                string[] messages = new string[]
                {
                    "{0}!! You will have to do better than that!",
                    "{0}!! Prepare to meet your doom!",
                    "{0}!! I will crush you!",
                    "{0}!! You will pay for that!",
                    "{0}!! I am a god, you really think you can hurt me!"
                };

                string message = String.Format(messages[Utility.Random(messages.Length)], from.Name);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, message);
            }
        }

        public RedDragonAncient(Serial serial)
            : base(serial)
        {
        }

public override void OnThink()
{
    base.OnThink();

    // Manage Disguise
    if (Combatant == null && !IsBodyMod && !Controlled && m_DisguiseTimer == null && Utility.RandomBool())
    {
        m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), Disguise);
    }

    if (Combatant is Mobile mobileCombatant && mobileCombatant.InRange(Location, 6))
    {
        RemoveDisguise();
    }

    // Check if the dragon can use its nuke ability, and if it's below 50% health
    if (Hits < (HitsMax * 0.5) && DateTime.UtcNow >= _nextNukeAllowed && Utility.RandomDouble() < 0.05) // 5% chance and cooldown check
    {
        DoNuke();
        _nextNukeAllowed = DateTime.UtcNow + NukeCooldown; // Set the next allowed time for the nuke
    }

    // Handle breath attack
    if (Combatant != null && DateTime.UtcNow >= m_NextBreathTime)
    {
        BreathSpecialAttack();
        m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Updated cooldown to 20 seconds
    }

    // Handle True Fear effect
    if (Combatant != null && InRange(Combatant, 8) && Combatant is Mobile mobileTarget)
    {
        if (DateTime.UtcNow >= _nextTrueFearAllowed)
        {
            ApplyTrueFear(mobileTarget);  // Apply True Fear to the target
            _nextTrueFearAllowed = DateTime.UtcNow + TrueFearCooldown; // Set the next allowed time
        }
    }
}
#region DoNuke
public void DoNuke()
{
    if (!Alive || Map == null)
        return;

    Say(1112362); // You will burn to a pile of ash!

    int range = 8;
    int baseDamage = 50; // Base damage for the nuke

    // Play initial sound
    Effects.PlaySound(Location, Map, 0x349);

    // Create flame columns in a circular pattern
    for (int i = 0; i < 2; i++)
    {
        Misc.Geometry.Circle2D(Location, Map, i, (pnt, map) =>
        {
            Effects.SendLocationParticles(EffectItem.Create(pnt, map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
        });
    }

    // Flash and boom effect
    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
    {
        if (Alive && Map != null)
        {
            Effects.PlaySound(Location, Map, 0x44B); // Play explosion sound

            // Create a flash effect
            Packet flash = ScreenLightFlash.Instance;
            IPooledEnumerable clients = Map.GetClientsInRange(Location, (range * 4) + 5);

            foreach (NetState ns in clients)
            {
                if (ns.Mobile != null)
                    ns.Mobile.Send(flash);
            }

            clients.Free();

            // Send explosion effects in a circular pattern
            for (int i = 0; i < range; i++)
            {
                Misc.Geometry.Circle2D(Location, Map, i, (pnt, map) =>
                {
                    Effects.SendLocationEffect(pnt, map, 14000, 14, 10, Utility.RandomMinMax(2497, 2499), 2);
                });
            }
        }
    });

    // Damage nearby targets
    IPooledEnumerable nearbyMobiles = GetMobilesInRange(range);

    foreach (Mobile m in nearbyMobiles)
    {
        if (m != null && m.Alive && CanBeHarmful(m))
        {
            // Apply damage after a slight delay
            Timer.DelayCall(TimeSpan.FromSeconds(1.75), new TimerStateCallback(DoDamage_Callback), m);
        }
    }

    nearbyMobiles.Free();
}

private void DoDamage_Callback(object state)
{
    Mobile target = state as Mobile;
    if (target != null && CanBeHarmful(target))
    {
        // Base damage range for the nuke
        int baseDamage = 50;

        // Get the target's fire resistance
        int fireResistance = target.GetResistance(ResistanceType.Fire); // Get resistance as a percentage

        // Calculate the reduced damage based on fire resistance
        double damageReductionFactor = 1.0 - (fireResistance / 100.0); // Convert resistance percentage to a reduction factor
        int finalDamage = (int)(baseDamage * damageReductionFactor); // Apply the reduction to the base damage

        // Apply the final damage to the target
        target.Damage(finalDamage, this);

        // Debugging statement
        Console.WriteLine($"Nuke damage applied: {finalDamage} to {target.Name}");
    }
}


    #endregion
     #region BreathSpecialAttack
public void BreathSpecialAttack()
{
    Map map = this.Map;
    if (map == null) return;

    // Play attack animation and sound
    this.Animate(11, 5, 1, true, false, 0);  // Attack1 animation
    this.PlaySound(0x227);                  // Fire breath sound

    Direction d = this.Direction;
    int range = 10; // Range of the cone in tiles
    int baseDamage = 45; // Base fire damage to be applied

    // Define the cone shape for the fire breath
    List<Point3D> fireTiles = new List<Point3D>();
    List<Mobile> affectedTargets = new List<Mobile>(); // List to track targets affected by damage

    int[] coneWidths = { 1, 1, 3, 3, 3, 5 }; // Cone width at each distance

    for (int i = 1; i <= range; i++)
    {
        int currentWidth = coneWidths[Math.Min(i - 1, coneWidths.Length - 1)];

        for (int j = -currentWidth / 2; j <= currentWidth / 2; j++)
        {
            int targetX = this.X;
            int targetY = this.Y;

            // Adjust tile positions based on the dragon's direction
            switch (d & Direction.Mask)
            {
                case Direction.North:
                    targetY -= i;
                    targetX += j;
                    break;
                case Direction.East:
                    targetX += i;
                    targetY += j;
                    break;
                case Direction.South:
                    targetY += i;
                    targetX += j;
                    break;
                case Direction.West:
                    targetX -= i;
                    targetY += j;
                    break;
                case Direction.Right: // North-East
                    targetX += i;
                    targetY -= i;
                    targetX += j;
                    break;
                case Direction.Down: // South-East
                    targetX += i;
                    targetY += i;
                    targetY += j;
                    break;
                case Direction.Left: // South-West
                    targetX -= i;
                    targetY += i;
                    targetX += j;
                    break;
                case Direction.Up: // North-West
                    targetX -= i;
                    targetY -= i;
                    targetX += j;
                    break;
            }

            if (map.CanFit(targetX, targetY, this.Z, 16, false, false))
            {
                fireTiles.Add(new Point3D(targetX, targetY, this.Z));

                // Check for mobiles in the range
                IPooledEnumerable nearbyMobiles = map.GetMobilesInRange(new Point3D(targetX, targetY, this.Z), 1);

                foreach (Mobile m in nearbyMobiles)
                {
                    if (m != null && m.Alive && CanBeHarmful(m))
                    {
                        // Calculate damage
                        double damageReductionFactor = 1.0 - (m.GetResistance(ResistanceType.Fire) / 100.0);
                        int finalDamage = (int)(baseDamage * damageReductionFactor);

                        // Apply damage after a slight delay
                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                        {
                            if (m.Alive && CanBeHarmful(m))
                                m.Damage(finalDamage, this);
                        });

                        affectedTargets.Add(m);
                    }
                }

                nearbyMobiles.Free();
            }
        }
    }

    // Apply fire field effect to the ground
    foreach (Point3D p in fireTiles)
    {
        SpawnFireField(p, map);
    }
}

private void SpawnFireField(Point3D location, Map map)
{
    // Ensure the map is valid before creating the fire field
    if (map == null)
        return;

    // Create a fire field at the given location on the map
    FireFieldSpell.FireFieldItem fireField = new FireFieldSpell.FireFieldItem(0x398C, location, this, map, TimeSpan.FromSeconds(30), 20);
}
#endregion
#region ApplyTrueFear
public void ApplyTrueFear(Mobile target)
{
    // Check if the target has high magic resist; if so, reduce the duration of the fear
    int fearDurationSeconds = (int)Math.Max(1, 13.0 - (target.Skills[SkillName.MagicResist].Value / 10.0));

    int localizedMessage;
    if (fearDurationSeconds <= 2)
        localizedMessage = 1080339; // A sense of discomfort passes through you, but it fades quickly.
    else if (fearDurationSeconds <= 4)
        localizedMessage = 1080340; // An unfamiliar fear washes over you...
    else if (fearDurationSeconds <= 7)
        localizedMessage = 1080341; // Panic grips you! You're unable to move...
    else if (fearDurationSeconds <= 10)
        localizedMessage = 1080342; // Terror slices into your being...
    else
        localizedMessage = 1080343; // Everything around you dissolves into darkness...

    // Send the localized fear message to the target
    target.SendLocalizedMessage(localizedMessage, this.Name, 0x21);
    
    PlaySound(0x525); 
    // Apply the fear effect (paralysis) to the target
    target.Frozen = true;

    // Add a debuff icon to represent True Fear
    BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.TrueFear, 1153791, 1153827, TimeSpan.FromSeconds(fearDurationSeconds), target));

    // After the fear duration, unfreeze the target
    Timer.DelayCall(TimeSpan.FromSeconds(fearDurationSeconds), () =>
    {
        target.Frozen = false;
        target.SendLocalizedMessage(1005603); // You can move again!
    });
}
#endregion
        public override bool ReacquireOnMovement
        {
            get { return true; }
        }

        public override int DefaultHitsRegen
        {
            get
            {
                int regen = base.DefaultHitsRegen;
                return IsParagon ? regen : regen + 40;
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool AutoDispel { get { return true; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Hides { get { return 40; } }
        public override int Meat { get { return 25; } }
        public override int Scales { get { return 20; } }
        public override ScaleType ScaleType
        {
            get
            {
                return (Body == 20 ? ScaleType.Yellow : ScaleType.Red);
            }
        }
        public override int DragonBlood { get { return 48; } }
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        public override int TreasureMapLevel { get { return 5; } }
        public override bool CanFly { get { return true; } }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();

            if (Combatant == null && !IsBodyMod && !Controlled && m_DisguiseTimer == null && Utility.RandomBool())
                m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), Disguise);
        }

        public override void OnActionWander()
        {
            base.OnActionWander();

            if (Combatant == null && !IsBodyMod && !Controlled && m_DisguiseTimer == null && Utility.RandomBool())
                m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), Disguise);
        }

        public override bool OnBeforeDeath()
        {
            RemoveDisguise();
            return base.OnBeforeDeath();
        }

        #region Disguise
        public void Disguise()
        {
            if (Combatant != null || IsBodyMod || Controlled)
                return;

            FixedEffect(0x376A, 8, 32);
            PlaySound(362);

            BodyMod = 0x191;
            Name = NameList.RandomName("female");
            Title = "the seductress";
            Hue = Race.Human.RandomSkinHue();
            HairItemID = Race.Human.RandomHair(this);
            HairHue = Race.Human.RandomHairHue();

            SetWearable(new Robe(), 1645);

            m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(300), RemoveDisguise); // Changed to 300 seconds (5 minutes)
        }

        public void RemoveDisguise()
        {
            Name = "a Ancient Red Wyrm";
            Title = null;
            PlaySound(362);

            if (IsBodyMod)
            {
                BodyMod = 826;
                Hue = 1645;
                HairItemID = 0;
                HairHue = 0;
                FacialHairItemID = 0;
                FacialHairHue = 0;
            }

            m_DisguiseTimer = null;
        }
        #endregion

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 8);
            AddLoot(LootPack.Gems, 10);
        }

        public override int GetIdleSound()
        {
            return 0x2D3;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
        